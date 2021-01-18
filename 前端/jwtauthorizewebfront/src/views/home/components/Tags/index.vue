<template>
  <div class="tags">
    <div class="tagsWrapper">
      <div
        class="tag"
        v-for="(tag, i) in tags"
        :class="{ active: $route.name === tag.name }"
        :key="tag.meta.title"
        @click="go(tag)"
      >
        {{ tag.meta.title }}
        <i
          class="icon el-icon-close"
          v-if="tag.name !== 'home'"
          @click.stop="remove(i)"
        ></i>
      </div>
    </div>
    <el-dropdown trigger="click">
      <div class="tagsCtrl">
        <i class="el-icon-s-operation"></i>
      </div>
      <el-dropdown-menu slot="dropdown">
        <el-dropdown-item @click.native="close($event, 'all')"
          >全部关闭</el-dropdown-item
        >
        <el-dropdown-item @click.native="close($event)"
          >关闭其它</el-dropdown-item
        >
        <el-dropdown-item divided @click.native="goHome">
          返回首页
        </el-dropdown-item>
      </el-dropdown-menu>
    </el-dropdown>
  </div>
</template>

<script>
export default {
  name: "Tags",
  data() {
    return {
      tags: [],
      tagsMap: [],
    };
  },
  computed: {
    menusMap() {
      var menusMap = this.$store.getters["global/menusMap"];
      return menusMap;
    },
  },
  watch: {
    menusMap() {
      if (this.menusMap["home"]) {
        let home = this.$route.matched[0];
        this.tags.unshift(home); //向开头添加一个元素
        this.tagsMap.push("home");
      }
      this.setTags();
    },
    $route() {
      this.setTags();
    },
  },
  methods: {
    setTags() {
      let route = this.$route;
      if (this.menusMap[route.name]) {
        route.meta.title = this.menusMap[route.name];
      }
      if (!this.tagsMap.includes(route.name)) {
        this.tags.push(route);
        this.tagsMap.push(route.name);
      }
    },
    go(tag) {
      if (tag.name == this.$route.name) return;
      this.$router.replace({ name: tag.name });
    },
    goHome() {
      this.$router.replace({ name: "home" });
    },
    close(e, type) {
      this.tags = [];
      this.tagsMap = [];
      if (type) {
        this.$router.replace({ name: "home" });
      } else {
        if (this.menusMap["home"]) {
          let home = this.$route.matched[0];
          this.tags.unshift(home);
          this.tagsMap.push("home");
        }
      }
      this.setTags();
    },
    remove(i) {
      if (i && i === this.tags.length - 1) {
        this.$router.replace({ name: this.tags[i - 1].name });
      } else {
        if (!this.tags[i + 1].name === this.$route.name) {
          this.$router.replace({ name: this.tags[i + 1].name });
        }
      }
      this.tags.splice(i, 1);
      this.tagsMap.splice(i, 1);
    },
  },
};
</script>

<style lang="scss" scoped>
.tags {
  display: flex;
  align-items: center;
  position: relative;
  font-size: 0;
  &:after {
    content: "";
    position: absolute;
    right: 0;
    bottom: 0;
    left: 0;
    border-bottom: 1px solid burlywood;
    transform: scaleY(0.5);
  }
  .tagsWrapper {
    flex: 1;
    white-space: nowrap;
    .tag {
      position: relative;
      padding: 10px 30px;
      display: inline-block;
      vertical-align: top;
      font-size: 14px;
      color: #707070;
      user-select: none;
      &:after {
        content: "";
        position: absolute;
        top: 0;
        right: 0;
        bottom: 0;
        border-right: 1px solid #eeeef0;
        transform: scaleY(0.5);
      }
      &.active:before {
        content: "";
        position: absolute;
        top: 50%;
        left: 10px;
        width: 6px;
        height: 6px;
        background-color: #1a63eb;
        box-shadow: 0 0 4px rgba(26, 99, 235, 0.4);
        border-radius: 50%;
        transform: translateY(-50%);
      }
      .icon {
        position: absolute;
        top: 50%;
        right: 8px;
        font-size: 12px;
        transform: translateY(-50%);
        cursor: pointer;
      }
    }
  }
  .tagsCtrl {
    position: relative;
    margin: 0 10px;
    padding: 0px;
    cursor: pointer;
    &:after {
      content: "";
      position: absolute;
      top: 0;
      right: 0;
      bottom: 0;
      border-right: 1px solid #1a63eb;
      transform: scaleY(0.5);
    }
  }
}
</style>