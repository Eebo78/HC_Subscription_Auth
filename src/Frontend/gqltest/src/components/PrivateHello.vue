<template>
  <ApolloQuery :query="require('../graphql/privateHello.gql')">
    <template v-slot="{ result: { loading, error, data } }">
      <!-- Loading -->
      <div v-if="loading" class="loading apollo">Loading...</div>

      <!-- Error -->
      <div v-else-if="error" class="error apollo">
        An error occurred: {{ error }}
      </div>

      <!-- Result -->
      <div v-else-if="data" class="result apollo">{{ data.privateHello }}</div>

      <!-- No result -->
      <div v-else class="no-result apollo">No result :(</div>
    </template>
  </ApolloQuery>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({})
export default class PrivateHello extends Vue {}
</script>
