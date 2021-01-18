
<template>
  <el-menu
    :default-active="$route.name"
    class="menu-vertical"
    background-color="#545c64"
    text-color="#fff"
    active-text-color="#ffd04b"
    :collapse="fold"
    unique-opened
    router
  >
    <template v-for="menu in menus">
      <el-menu-item
        v-if="!menu.children.length"
        :index="menu.module"
        :key="menu.Id"
      >
        <i :class="menu.icon"></i>
        <span slot="title">{{ menu.name }}</span>
      </el-menu-item>

      <el-submenu v-else :index="menu.module" :key="menu.Id">
        <template slot="title">
          <i :class="menu.icon"></i>
          <span slot="title">{{ menu.name }}</span>
        </template>
        <el-menu-item
          v-for="son in menu.children"
          :index="son.module"
          :key="son.Id"
          >{{ son.name }}</el-menu-item
        >
      </el-submenu>
    </template>
  </el-menu>
</template>


<script>
export default {
  name: "NavMenu",
  computed: {
    menus() {
      return this.$store.getters["global/menus"];
    },
    fold() {
        //:collapse="fold" 	是否水平折叠收起菜单（仅在 mode 为 vertical 时可用）
      return this.$store.getters["global/fold"];
    },
  },
};
</script>

<style lang="scss" scoped>
.el-menu {
  height: 100%;
  border-right: 0;
  &.menu-vertical:not(.el-menu--collapse) {
    width: 200px;
  }
  li[role="menuitem"] {
    border-top: 1px solid #37414b;
    border-bottom: 1px solid #1f262d;
    &:nth-child(1) {
      border-top: none;
    }
    &:nth-last-child(1) {
      border-bottom: none;
    }
  }
  .el-menu--inline {
    .el-menu-item {
      border: none;
      background-color: #2b2c2c !important;
    }
  }
}
</style>

