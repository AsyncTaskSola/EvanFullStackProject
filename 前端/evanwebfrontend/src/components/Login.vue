<template>
  <div class="login_container">
    <div class="login_box">
      <div class="logo_box">
        <img src="../assets/logo.png" alt="" />
      </div>

      <!-- 登陆 -->
      <el-form
        ref="LoginFormRef"
        :model="loginForm"
        :rules="LoginFormRules"
        label-width="0px"
        class="login_form"
      >
        <!-- 账号名 -->
        <el-form-item prop="username">
          <el-input
            v-model="loginForm.username"
            prefix-icon="el-icon-user-solid"
          ></el-input>
        </el-form-item>
        <!--acessToken -->
        <el-form-item prop="password">
          <el-input
            v-model="loginForm.password"
            prefix-icon="el-icon-lock"
            type="password"
          ></el-input>
        </el-form-item>

        <el-form-item class="btns">
          <!-- 预验证 -->
          <el-button type="primary" @click="Login">登陆</el-button>
          <el-button type="info" @click="ResetLoginForm">重置</el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      loginForm: {
        username: "Alice Smith",
        password: "",
      },
      //验证规则
      LoginFormRules: {
        username: [
          { required: true, message: "请输入登陆名称", trigger: "blur" },
          {
            min: 3,
            max: 15,
            message: "长度在 3 到 15 个字符",
            trigger: "blur",
          },
        ],
        password: [
          { required: true, message: "请输入accssToken", trigger: "blur" },
          {
            min: 3,
            max: 1000,
            message: "长度在 3 到 1000 个字符,请准确输入",
            trigger: "blur",
          },
        ],
      },
    };
  },
  methods: {
    //   重置
    ResetLoginForm() {
      this.$refs.LoginFormRef.resetFields();
      this.loginForm.username = "";
      this.loginForm.password = "";
    },
    //预验证
    Login() {
      this.$refs.LoginFormRef.validate((res) => {
        console.log("预验证", res);
        if (res) {
          this.$http
            .post(
              "/api/LoginUserInfo/GetInfo",
              {},
              {
                headers: {
                  Authorization: `Bearer ${this.loginForm.password}`,
                },
              }
            )
            .then((res) => {
              console.log(res);
              if (!res) {
                return this.$message.error("请输入正确数据");
              }
              this.loginForm.username = res.data.data.username;
              if ((res.data.status != 200)) {
                console.log("登陆失败");
              }
            })
            .catch((err) => {
              if (err.message.indexOf("401") !== -1) {
                console.log("accessToken不正确或已过期");
              }
            });
        }
      });
    },
  },
};
</script>

<style lang="less" scoped>
.login_container {
  display: flex;
  background-color: burlywood;
  width: 100%;
  height: 100%;
  justify-content: center;
  align-items: center;
}
.login_box {
  width: 450px;
  height: 300px;
  background-color: #fff;
  border-radius: 10px;
  position: relative;
  .logo_box {
    height: 100px;
    width: 100px;
    border: 1px solid #eee;
    border-radius: 50%;
    padding: 10px;
    box-shadow: 0 0 3px #ddd;
    position: absolute;
    transform: translate(-50%, -50%);
    background-color: #fff;
    left: 50%;
    img {
      width: 100px;
      height: 100px;
      border-radius: 50%;
      background-color: #eee;
    }
  }
}
.btns {
  display: flex;
  justify-content: flex-end;
}
.login_form {
  position: absolute;
  margin-top: 100px;
  width: 100%;
  padding: 0 20px;
  box-sizing: border-box;
}
</style>