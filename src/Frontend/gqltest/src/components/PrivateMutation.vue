<template>
  <ApolloMutation :mutation="require('../graphql/privateMutation.gql')" :update="response">
    <template v-slot="{ mutate, loading, error }">
      <button :disabled="loading" @click="mutate()">Trigger private mutation</button>
      <!-- Loading -->
      <div v-if="loading" class="loading apollo">Loading...</div>

      <!-- Error -->
      <div v-else-if="error" class="error apollo">An error occurred</div>

      <!-- Result -->
      <div v-else-if="mutationResult" class="result apollo">{{ mutationResult }}</div>
    </template>
  </ApolloMutation>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { InMemoryCache } from "apollo-cache-inmemory";

@Component({})
export default class PrivateMutation extends Vue {
  private response(
    store: InMemoryCache,
    { data: { privateMutation } }: { data: { privateMutation: string } }
  ) {
    this.mutationResult = privateMutation;
  }

  private mutationResult = "";
}
</script>
