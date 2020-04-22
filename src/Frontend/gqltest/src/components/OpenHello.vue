<template>
  <ApolloQuery :query="require('../graphql/openhello.gql')">
    <ApolloSubscribeToMore
      :document="require('../graphql/openSub.gql')"
      :updateQuery="onOpenSubEvent"
    />
    <ApolloSubscribeToMore
      :document="require('../graphql/privateSub.gql')"
      :updateQuery="onPrivateSubEvent"
    />

    <template v-slot="{ result: { loading, error, data } }">
      <!-- Loading -->
      <div v-if="loading" class="loading apollo">Loading...</div>

      <!-- Error -->
      <div v-else-if="error" class="error apollo">An error occurred</div>

      <!-- Result -->
      <div v-else-if="data" class="result apollo">
        <div>Open query result: {{ data.hello }}</div>

        <div>Open subscription result: {{ subresult }}</div>

        <div>Private subscription result: {{ privateSubresult }}</div>
      </div>
      <!-- No result -->
      <div v-else class="no-result apollo">No result :(</div>
    </template>
  </ApolloQuery>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({})
export default class OpenHello extends Vue {
  private onOpenSubEvent(previousResult: any, { subscriptionData }: any) {
    this.subresult = subscriptionData.data.openSubscription;
  }
  private onPrivateSubEvent(previousResult: any, { subscriptionData }: any) {
    this.privateSubresult = subscriptionData.data.privateSubscription;
  }

  private subresult: string | null = null;
  private privateSubresult: string | null = null;
}
</script>
