<template>
  <div
    class="userItem"
    @click="handleClick"
    :class="{ active: currentUser.id && user.id === currentUser.id }"
  >
    <div class="avatar">
      <el-avatar :src="user.avatar ? user.avatar : userJpg" fit="cover" />
    </div>
    <div class="userInfo">
      <p class="name">{{ user.name }}</p>
      <div class="roles">
        <el-tag v-for="role in user.roles" :key="role.id" size="mini">
          {{ role.name }}
        </el-tag>
      </div>
    </div>
    <div class="status" :class="{ online: user.status }"></div>
  </div>
</template>

<script>
import userJpg from "@/assets/images/user.jpeg";
export default {
  name: "UsersItem",
  props: ["user"],
  data() {
    return {
      userJpg,
    };
  },

  computed: {
    currentUser() {
      var result = this.$store.getters["user/currentUser"];
      return result;
    },
  },
  methods: {
    handleClick() {
      if (this.currentUser.id === this.user.id) return; // 本来是有的
      this.$emit("getUser", this.user.id);
    },
  },
};
</script>
<style lang='scss' scoped>
.userItem {
  padding: 10px;
  position: relative;
  display: flex;
  &:hover {
    cursor: pointer;
    margin-bottom: 20px;
    background-color: rgb(223, 244, 245);
    border-radius: 10px;
  }
  &.active {
    background-color: #eeeef0;
  }
  &:after {
    content: "";
    position: absolute;
    right: 10px;
    bottom: 0;
    left: 10px;
    border-bottom: 1px solid #eeeef0;
    transform: scaleY(0.5);
  }
  .userInfo {
    flex: 1;
    margin-left: 8px;
    display: flex;
    flex-direction: column;
    flex-wrap: wrap; //一行显示不完，自动换行
    .name {
      font-size: 14px;
      margin-bottom: 2px;
      color: #707070;
    }
    .roles {
      /deep/.el-tag {
        margin-right: 4px;
        &:nth-last-child(1) {
          margin: 0;
        }
      }
    }
  }
  .status {
    margin-left: 8px;
    width: 10px;
    height: 10px;
    border-radius: 50%;
    box-shadow: 0 0 7px rgba(238, 238, 240, 0.4);
    &.online {
      background-color: #67c23a;
      box-shadow: 0 0 7px rgba(103, 194, 58, 0.4);
    }
  }
}
</style>