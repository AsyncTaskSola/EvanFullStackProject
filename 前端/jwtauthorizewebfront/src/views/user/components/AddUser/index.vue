<template>
  <div class="AddUser">
    <div class="container">
      <div class="title">
        <h4>添加用户</h4>
        <div class="iconWrapper" @click="close">
          <i class="icon el-icon-close"></i>
        </div>
      </div>
      <div class="avatar">
        <el-avatar :src="userJpg" fit="cover" :size="120"></el-avatar>
      </div>

      <div class="content">
        <el-form
          :model="ruleForm"
          :rules="rules"
          ref="ruleForm"
          label-width="100px"
          class="demo-ruleForm"
        >
          <el-form-item label="名称" prop="name" size="mini">
            <el-input v-model="ruleForm.name"></el-input>
          </el-form-item>
          <el-form-item label="登陆账号" prop="account" size="mini">
            <el-input v-model="ruleForm.account"></el-input>
          </el-form-item>
          <el-form-item label="登陆密码" prop="pwd" size="mini">
            <el-input v-model="ruleForm.pwd"></el-input>
          </el-form-item>
        </el-form>
      </div>
    </div>

    <!-- 侧面的按钮 -->
    <CtrlButton
      :Showbutton="showbutton"
      :Button="buttons"
      @Oclick="VualeChick"
    ></CtrlButton>

    <RoleList
      :showRolesList="showRoles"
      @showrole="XResult"
      :CurrentInfo="Currentdata"
      @CheckCurrentRole="Role"
    >
    </RoleList>
  </div>
</template>

<script>
import userJpg from "@/assets/images/user.jpeg";
import RoleList from "../RoleList";
export default {
  name: "AddUser",
  components: {
    RoleList,
  },
  data() {
    return {
      userJpg,
      ruleForm: {
        name: "",
        account: "",
        pwd: "",
      },
      rules: {
        name: [
          { required: true, message: "请输入名称", trigger: "blur" },
          {
            min: 3,
            max: 16,
            message: "长度在 3 到 16 个字符",
            trigger: "blur",
          },
        ],
        account: [
          {
            required: true,
            message: "请输入登陆账号",
            trigger: "blur",
          },
          {
            min: 3,
            max: 16,
            message: "长度至少3位,最长16位",
            trigger: "blur",
          },
        ],
        pwd: [
          {
            required: true,
            message: "请输入登陆密码",
            trigger: "blur",
          },
          {
            min: 6,
            max: 16,
            message: "长度至少6位,最长16位",
            trigger: "blur",
          },
        ],
      },
      Currentdata: "",
      showbutton: false,
      showRoles: false,
      buttons: [
        {
          icon: "el-icon-user",
          type: "primary",
          name: "选择权限",
          methods: "edit",
        },
        {
          icon: "el-icon-refresh",
          type: "warning",
          name: "重置",
          methods: "reset",
        },
        {
          icon: "el-icon-check",
          type: "info",
          name: "提交",
          methods: "sumbit",
        },
      ],
      CheckRole: {},
      Roles: [],
    };
  },
  methods: {
    VualeChick(n) {
      switch (n) {
        case "edit": {
          this.showRoles = true;
          break;
        }
        case "sumbit": {
          this.$refs["ruleForm"].validate(async (valid) => {
            if (valid) {
              let res = JSON.stringify(this.CheckRole);
              if (res == "{}") {
                this.$message({
                  message: "请选择权限",
                  type: "warning",
                });
              } else {
                var AddF = this.ruleForm;
                var V_user = {
                  Name: AddF.name,
                  Account: AddF.account,
                  Password: AddF.pwd,
                  Roles: this.Roles,
                };
                await this.$store.dispatch("user/AddUser", V_user);
                setTimeout(async () => {
                  this.$store.commit("user/resetCurrentUser");
                  await this.$store.dispatch("user/getUsers", {
                    pageindex: 1,
                    pageSize: 10,
                  });
                }, 100);
                return this.$store.getters["user/users"];
              }
            } else {
              console.log("error submit!!");
              return false;
            }
          });
        }
        case "reset": {
          this.$refs["ruleForm"].resetFields();
        }
      }
    },
    close() {
      this.$store.commit("user/resetCurrentUser");
    },
    XResult(ary) {
      ary = false;
      this.showRoles = ary;
    },
    Role(arg) {
      this.CheckRole = arg;
      console.log("emit", this.CheckRole);
      this.Roles.push(this.CheckRole.id);
    },
  },
  //dom元素加载完成
  mounted() {
    this.showbutton = true;
  },
};
</script>

<style lang="scss" scoped>
.AddUser {
  display: flex;
  height: 100%;
  width: 100%;
  // justify-content: center;
  .container {
    border-radius: 20px;
    height: 423px;
    background-color: rgb(255, 255, 255);
    .title {
      position: relative;
      display: flex;
      right: 0;
      h4 {
        flex: 1;
        margin-top: 15px;
        margin-bottom: 10px;
        font-size: 16px;
        text-align: center;
        color: indigo;
      }
      &:hover {
        .iconWrapper .icon {
          display: block;
        }
      }
      .iconWrapper {
        justify-content: center;
        align-content: center;
        height: 15px;
        width: 15px;
        background-color: rgb(243, 86, 86);
        border-radius: 50%;
        .icon {
          display: none;
          cursor: pointer;
          font-size: 15px;
        }
      }
    }
    .avatar {
      display: flex;
      justify-content: center;
      align-items: center;
    }
    .content {
      margin-top: 40px;
    }
    /deep/ .el-form-item__label {
      text-align-last: justify;
    }
  }
}
</style>