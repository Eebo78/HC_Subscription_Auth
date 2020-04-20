<template>
  <div>
    <div>{{ subResult }}</div>
  </div>
</template>

<script>
import gql from "graphql-tag";

export default {
  name: "PrivateSub",
  data() {
    return { subResult: "" };
  },
  apollo: {
    $subscribe: {
      onUpdate: {
        query: gql`
          subscription closedSub {
            onSomeThingHappendSubscription
          }
        `,
        result({ data }) {
          console.log(data);
          this.subResult = data.onSomeThingHappendSubscription;
        }
      }
    }
  }
};
</script>
