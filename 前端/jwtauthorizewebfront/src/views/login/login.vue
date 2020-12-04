<template>
  <div id="login" class="pageContainer">
    <div class="loginContainer">
      <div class="loginWrapper">
        <h1>JWT-RABC</h1>
        <div class="formWrapper">
          <el-form :model="form" :rules="rules" ref="form">
            <el-form-item prop="account">
              <input
                type="text"
                :readonly="!accountShow"
                placeholder="账号"
                v-model="form.account"
                @click="accountShow = true"
                @blur="accountShow = false"
                @keydown.enter="submitForm('form')"
              />
            </el-form-item>

            <el-form-item prop="password">
              <div class="passwordWrapper">
                <input
                  v-focus
                  :type="pwdType"
                  @input="pwdView ? (pwdType = 'text') : (pwdType = 'password')"
                  placeholder="密码"
                  v-model="form.password"
                  @keydown.enter="submitForm('form')"
                />
                <i
                  v-if="form.password"
                  class="el-icon-view"
                  :class="{ pwdView }"
                  @click="pwdDiplay"
                ></i>
              </div>
            </el-form-item>

            <el-form-item>
              <div class="button" @click="submitForm('form')">
                <i v-if="loading" class="el-icon-loading"></i>
                {{ loading ? "登陆中" : "登陆" }}
              </div>
              <div class="button" @click="forgetForm()">遗忘</div>
            </el-form-item>
          </el-form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
let self;
export default {
  name: "login",
  //自定义指令
  directives: {
    focus: {
      inserted: function (el) {
        if (self.form.account) {
          // 账号存在，密码框自动聚焦
          el.focus();
        }
      },
    },
  },
  created() {
    self = this;
  },
  data() {
    return {
      pwdType: "text",
      pwdView: false,
      accountShow: false,
      loading: false,
      form: {
        account: localStorage.getItem("Account")
          ? localStorage.getItem("Account")
          : "",
        password: "",
      },
      rules: {
        account: [
          { required: true, message: "请输入账号", trigger: "blur" },
          {
            min: 3,
            max: 16,
            message: "长度在 3 到 16 个字符",
            trigger: "blur",
          },
        ],
        password: [
          { required: true, message: "请输入密码", trigger: "blur" },
          {
            min: 6,
            max: 16,
            message: "长度至少6位,最长16位",
            trigger: "blur",
          },
        ],
      },
    };
  },
  methods: {
    submitForm(formName) {
      if (this.loading) return;
      console.log("refs", this.$refs);
      this.$refs[formName].validate(async (valid) => {
        console.log(valid);
        if (valid) {
          this.loading = true;
          var res = await this.$store.dispatch("user/login", this.form);
          if (res) {
            this.$router.push("/home");
            this.$destroy();
          } else {
            this.loading = false;
          }
        } else {
          return false;
        }
      });
    },
    //点击查看密码
    pwdDiplay() {
      this.pwdView = !this.pwdView;
      this.pwdView ? (this.pwdType = "text") : (this.pwdType = "password");
    },
    forgetForm() {
      var name = this.form.account;
      this.$store.dispatch("user/setUserResetPassword", name);
    },
  },

  beforeRouteEnter(to, from, next) {
    next((vm) => {
      const result = sessionStorage.getItem("userInfo");
      console.log("login 下beforeRouteEnter方法", result);
      if (sessionStorage.getItem("userInfo")) {
        vm.$store.commit("global/errorRes", { status: false, msg: "" });
        let userInfo = JSON.parse(sessionStorage.getItem("userInfo"));
        vm.$store.dispatch("user/logout", userInfo.id);
      }
    });
  },
};
</script>

<style lang="scss" scoped>
@import "../../assets/css/login.scss";
</style>