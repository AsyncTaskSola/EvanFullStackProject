<template>
  <div class="userInfo">
    <el-dropdown trigger="click" size="small">
      <div class="avatar">
        <el-avatar shape="square" fit="cover" :size="50" :src="user.avatar"></el-avatar>
      </div>
      <el-dropdown-menu slot="dropdown">
        <el-dropdown-item @click.native="dialog = true"
          >个人设置</el-dropdown-item
        >
        <el-dropdown-item @click.native="logout">退出登陆</el-dropdown-item>
      </el-dropdown-menu>
    </el-dropdown>
    <div class="info">
      <p class="name">{{ user.name }}</p>
      <div class="roles">
        <el-tabs v-for="r in user.roles" :key="r.id" size="mini">
          {{ r.name }}
        </el-tabs>
      </div>
    </div>

    <!-- 弹出个人设置 -->
    <el-dialog title="个人设置" :visible.sync="dialog" destroy-on-close append-to-body center width="300px">
        <UpdateUser :visible.sync="dialog" />
    </el-dialog>
  </div>
</template>


<script>
import UpdateUser from './UpdateUser/index'
export default {
    components:{
        UpdateUser
    },
  computed: {
    user() {
      var userInfo = this.$store.getters["user/userInfo"];
      console.log("userInfo", userInfo);
      return userInfo;
    },
  },
  data() {
    return {
      dialog: false,
    };
  },
  methods: {
    logout() {
       this.$router.replace({name:'login'})
    },
  },
};
</script>

<style lang="scss" scoped>
.userInfo {
  display: flex;
  align-content: center;
  .avatar {
    margin-right: 4px;
    position: relative;
    cursor: pointer;
  }
  .info {
    display: flex;
    flex-direction: column;
    .name {
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
}
</style>