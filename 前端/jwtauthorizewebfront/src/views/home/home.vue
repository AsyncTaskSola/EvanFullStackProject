<template>
  <div id="home" class="pageContainer">
    <div class="menuWrapper">
      <NavMenu />
    </div>
    <div class="homeContainer">
      <div class="pageInfo">
          <TopBar />
      </div>
      <Tags />
      <div class="homeWrapper">
        <div class="pageContainer">
        <router-view></router-view>

        </div>
      </div>
    </div>
  </div>
</template>

<script>
import NavMenu from "./components/NavMenu/index";
import TopBar from "./components/TopBar/index";
import Tags from "./components/Tags/index"
export default {
  name:'home',
  components: {
    NavMenu,
    TopBar,
    Tags
  },
  computed: {
    userInfo() {
      return this.$store.getters["user/userInfo"];
    },
  },
  data() {
    return {};
  },
  async created() {    
    this.$notify({
      title: this.userInfo.name,
      type: "success",
      message: "欢迎回来",
      duration: 2000,
      showClose: false,
    });
    this.$store.dispatch("global/setMenus");
  },
};
</script>

<style lang="scss" scoped>
@import "@/assets/css/home.scss";
</style>