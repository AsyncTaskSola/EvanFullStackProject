<template>
  <div class="userInfo">
    <div class="container">
      <div class="wrapper">
        <div class="titleWrapper">
          <div class="container">
            <h4>用户资料</h4>
            <div class="iconWrapper" @click="close">
              <i class="icon el-icon-close"></i>
            </div>
          </div>
        </div>
        <div class="avatar">
          <el-avatar
            :size="100"
            fit="cover"
            :src="UserInfodata.avatar ? UserInfodata.avatar : userJpg"
          ></el-avatar>
          <p class="name">{{ UserInfodata.name }}</p>
        </div>
        <div class="infos">
          <Cell label="账号" :value="UserInfodata.account" />
          <Cell label="创建人" :value="UserInfodata.creator" />
          <Cell label="创建日期" :value="createDate" />
          <Cell label="上次登陆" :value="lastDate" />
        </div>
        <div class="roles">
          <h4>权限角色</h4>
          <div class="role" v-for="role in UserInfodata.roles" :key="role.id">
            <p class="name">{{ role.name }}</p>
            <p class="code">{{ role.code }}</p>
            <p class="describe">{{ role.describe }}</p>
          </div>
        </div>
      </div>
    </div>
    <!-- Oclick 子组件传过来的点击带参值 -->
    <CtrlButton
      :Showbutton="showbutton"
      :Button="buttons"
      @Oclick="VualeChick"
    ></CtrlButton>

    <RoleList
      :showRolesList="showRoles"
      @showrole="XResult"
      :showClickRole="showClickRoles"
      :CurrentInfo="this.UserInfodata"
    >
    </RoleList>
  </div>
</template>

<script>
import userJpg from "@/assets/images/user.jpeg";
import RoleList from "../RoleList";
export default {
  name: "UserInfo",
  props: ["UserInfodata"],
  components: {
    RoleList,
  },
  data() {
    return {
      userJpg,
      createDate: "",
      lastDate: "",
      showbutton: false,
      showRoles: false,
      showClickRoles: false,
      buttons: [
        {
          icon: "el-icon-edit",
          type: "primary",
          name: "编辑权限",
          methods: "edit",
        },
        {
          icon: "el-icon-refresh",
          type: "warning",
          name: "重置密码",
          methods: "reset",
        },
        {
          icon: "el-icon-close",
          type: "info",
          name: "禁用用户",
          methods: "disable",
        },
        {
          icon: "el-icon-delete",
          type: "danger",
          name: "删除用户",
          methods: "delete",
        },
      ],
    };
  },
  methods: {
    close() {
      this.$store.commit("user/resetCurrentUser");
    },
    DateChange() {
      this.createDate = this.UserInfodata.createDate.split("T")[0];
      this.lastDate = this.UserInfodata.lastDate.split("T")[0];
    },
    VualeChick(args) {
      switch (args) {
        case "delete":
          this.deleteUser();
          break;
        case "reset":
          this.resetPassword();
          break;
        case "disable":
          this.disable();
          break;
        case "edit":
          this.showRoles = true;
          // this.showClickRoles=true;//自动选择当前角色
          break;
      }
    },
    async deleteUser() {
      if (this.UserInfodata.isInit) {
        this.$message({ type: "error", message: "初始用户不能被删除" });
        return;
      }
      var res = await this.$store.dispatch(
        "user/deleteUser",
        this.UserInfodata.id
      );
      if ((res.message = "删除成功")) {
        this.close();
      }
      await this.$store.commit("user/resetCurrentUser");
      this.$store.dispatch("user/getUsers", { pageindex: 1, pageSize: 10 });
      return this.$store.getters["user/users"];
    },
    resetPassword() {
      this.$store.dispatch("user/setUserResetPassword", this.UserInfodata.name);
    },
    disable() {
      if (this.UserInfodata.isInit) {
        this.$message({ type: "error", message: "初始用户不能禁用" });
        return;
      }
      this.$store.dispatch("user/SetDisable", this.UserInfodata.id);
    },

    XResult(ary) {
      ary = false;
      this.showRoles = ary;
    },
  },
  //dom元素加载完成
  mounted() {
    this.showbutton = true;
  },
  created() {
    this.DateChange();
  },
};
</script>

<style lang="scss" scoped>
.userInfo {
  width: 100%;
  height: 100%;
  //background-color: wheat;
  display: flex;
  .container {
    position: relative;
    flex: 0 0 300px;
    //background-color: #f8f6f6;
    background-color: white;
    border-radius: 20px;
    align-self: flex-start;
    .wrapper {
      position: relative;
    }
    .titleWrapper {
      position: relative;
      margin-bottom: 10px;
      user-select: none;
      &:after {
        content: "";
        position: absolute;
        right: 0;
        left: 0;
        bottom: 0;
        border-bottom: 1px solid rgb(236, 209, 209);
        transform: scaleY(0.5);
        z-index: 2;
      }
      &:hover {
        .iconWrapper .icon {
          display: block;
        }
      }
      h4 {
        font-size: 14px;
        text-align: center;
        color: purple;
      }
      .iconWrapper {
        display: flex;
        justify-content: center;
        align-content: center;
        position: absolute;
        top: 30%;
        right: 0;
        height: 15px;
        width: 15px;
        background-color: rgb(245, 94, 94);
        border-radius: 50%;
      }
      .icon {
        display: none;
        cursor: pointer;
        font-size: 14px;
      }
    }
    .avatar {
      text-align: center;
      .name {
        font-size: 14px;
        color: purple;
        margin-top: 10px;
      }
    }
    .roles {
      position: relative;
      h4 {
        text-align: center;
        margin-top: 10px;
        font-size: 14px;
        color: purple;
      }
      .role {
        display: flex;
        justify-content: center;
        align-items: center;
        p {
          margin-top: 20px;
          font-size: 14px;
          flex: 1;
          text-align: center;
          margin-bottom: 15px;
        }
        p.code {
          color: cornflowerblue;
        }
      }
    }
  }
}

// .userInfo {
//   width: 100%;
//   height: 100%;
//   display: flex;
//   .container {
//     position: relative;
//     flex: 0 0 300px;
//     background-color: #96d8f7;
//     border-radius: 30px;
//     align-self: flex-start;
//     z-index: 2;
//     .wrapper {
//       position: relative;
//       .titleWrapper {
//         position: relative;
//         margin-bottom: 10px;
//         user-select: none;
//         &:after {
//           content: "";
//           position: absolute;
//           left: 0;
//           right: 0;
//           bottom: 0;
//           border-bottom: 1px solid wheat;
//           transform: scaleY(0.5);
//           z-index: 2;
//         }
//         &:hover {
//           .iconWrapper .icon {
//             display: block;
//           }
//         }
//         h4 {
//           font-size: 14px;
//           text-align: center;
//           color: #707070;
//           font-weight: normal;
//         }
//       }
//     }
//   }
// }
</style>