<template>
  <div>
    <!-- <h3>Open hello:</h3>
    <open-hello /> -->
    <h3>Private hello:</h3>
    <private-hello />
    <h3>OpenSub:</h3>
    <open-sub />
    <h3>Private Sub:</h3>
    <private-sub />
  </div>
</template>

<script>
// import OpenHello from "@/components/OpenHello.vue";
import PrivateHello from "@/components/PrivateHello.vue";
import OpenSub from "@/components/OpenSub.vue";
import PrivateSub from "@/components/PrivateSub.vue";

export default {
  name: "Home",
  components: {
    // OpenHello,
    PrivateHello,
    OpenSub,
    PrivateSub,
  },
};
</script>
