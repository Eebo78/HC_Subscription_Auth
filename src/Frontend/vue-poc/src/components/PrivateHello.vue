<template>
  <div>
    <div v-if="$apollo.loading">Loading...</div>
    <div>{{ privateHello }}</div>
  </div>
</template>

<script>
import gql from "graphql-tag";

export default {
  name: "PrivateHello",
  apollo: {
    // Simple query that will update the 'hello' vue property
    privateHello: gql`
      query {
        privateHello
      }
    `,
  },
};
</script>
