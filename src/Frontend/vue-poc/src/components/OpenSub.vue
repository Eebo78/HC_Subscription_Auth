<template>
  <div>
    <div>{{ subResult }}</div>
  </div>
</template>

<script>
import gql from "graphql-tag";

export default {
  name: "OpenSub",
  data() {
    return { subResult: "" };
  },
  apollo: {
    $subscribe: {
      onUpdate: {
        query: gql`
          subscription openSub {
            doItSubscription
          }
        `,
        result({ data }) {
          console.log(data);
          this.subResult = data.doItSubscription;
        },
      },
    },
  },
};
</script>
