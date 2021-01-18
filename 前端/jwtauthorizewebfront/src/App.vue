<template>
  <div id="app">
    <transition :name="pageAnimation">
    <!-- <keep-alive :include="keepAlive"> -->
    <router-view />
    <!-- </keep-alive> -->
    </transition>
  </div>
</template>

<script>
export default {
  name: "app",
  data() {
    return {
      pageAnimation: "",
    };
  },
  watch: {
    $route(to, from) {
      if (
        sessionStorage.getItem("Authorization") &&
        sessionStorage.getItem("Authorization") !== "undefined"
      ) {
        console.log("route", to);
        console.log("route", from);
        if (to.meta.index > from.meta.index) {
          this.pageAnimation = "slide-left";
        } else {
          this.pageAnimation = "slide-right";
        }
      }
    },
  },
};
</script>

<style lang="scss">
@import "@/assets/css/app.scss";
@import "@/assets/css/pageAnimation.scss";
</style>
