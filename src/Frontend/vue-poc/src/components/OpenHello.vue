<template>
  <div>
    <div v-if="$apollo.loading">Loading...</div>
    <div>{{ hello }}</div>
  </div>
</template>

<script>
import gql from "graphql-tag";

export default {
  name: "OpenHello",
  apollo: {
    // Simple query that will update the 'hello' vue property
    hello: gql`
      query {
        hello
      }
    `,
  },
};
</script>
