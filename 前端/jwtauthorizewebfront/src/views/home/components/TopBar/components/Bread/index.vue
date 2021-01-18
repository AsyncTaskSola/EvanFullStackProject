<template>
  <el-breadcrumb separator-class="el-icon-arrow-right">
    <transition-group name="slide-left">
      <template v-for="(x, i) in levelList">
        <el-breadcrumb-item
          v-if="x.meta.middle || i === levelList.length - 1"
          :key="x.name"
        >
          {{ x.meta.title }}
        </el-breadcrumb-item>

        <el-breadcrumb-item v-else :key="x.name" :to="x.path">
          {{ x.meta.title }}
        </el-breadcrumb-item>
      </template>
    </transition-group>
  </el-breadcrumb>
</template>

<script>
export default {
  name: "Bread",
  data() {
    return {
      levelList: [],
      levelMap: {},
    };
  },
  computed: {
    menus() {
      return this.$store.getters["global/menus"];
    },
    menusMap() {
      return this.$store.getters["global/menusMap"];
    },
  },
  watch: {
    menus(n) {
      this.getBreadcrumb();
    },

    $route() {
      this.getBreadcrumb();
    },
  },
  methods: {
    //Vue中面包屑导航功能和实现---面包屑导航--$route.matche
    getBreadcrumb() {
      //this.$route.matched 当前的路由
      let route = this.$route.matched.filter((item) => {
        if (item.name) {
          return true; //filter里面return true的时候，代表将当前item加入数组,而不是返回
        }
      });
      route.forEach((x) => {
        if (this.menusMap[x.name]) {
          x.meta.title = this.menusMap[x.name];
        }
      });
      this.levelList = route;
    },
  },
};
</script>

<style>
</style>