<template>
  <el-form :model="form" :rules="rules" ref="form" label-width="80px" >
    <el-upload
      class="avatar-uploader"
      action="#"
      :show-file-list="false"
      :auto-upload="false"
      :on-change="avatarChange"
    >
      <div class="avatarWrpper">
        <img v-if="imageUrl" :src="imageUrl" class="avatar" />
        <i v-else class="el-icon-plus avatar-uploader-icon"></i>
      </div>
    </el-upload>

    <el-form-item label="旧密码" prop="oldPassword" size="mini">
      <el-input type="password" v-model="form.oldPassword"></el-input>
    </el-form-item>
    <el-form-item label="新密码" prop="newPassword" size="mini">
      <el-input type="password" v-model="form.newPassword"></el-input>
    </el-form-item>
    <el-form-item label="确定密码" prop="checkPass" size="mini">
      <el-input type="password" v-model="form.checkPass"></el-input>
    </el-form-item>

    <div class="buttonWrapper">
      <el-button size="mini" @click="cancel">取消</el-button>
      <el-button type="primary" size="mini" @click="submit">确定</el-button>
    </div>
  </el-form>
</template>

<script>
export default {
  props: ["visible"],
  name: "UpdateUser",
  data() {
    //https://www.zhihu.com/question/357642812/answer/909168472
    var validateOldPass = (rule, value, callback) => {
      if (this.form.newPassword && !value) {
        callback(new Error("请输入旧密码"));
      } else {
        callback();
      }
    };
    var validateCheckPass = (rule, value, callback) => {
      //value 相当于当前框的值
      if (this.form.newPassword && !value) {
        callback(new Error("请再次输入密码"));
      } else if (value !== this.form.newPassword) {
        callback(new Error("两次输入密码不一致!"));
      } else {
        callback();
      }
    };
    return {
      imageUrl: "",
      form: {
        id: null,
        oldPassword: "",
        newPassword: "",
        checkPass: "",
        name: null,
      },
      rules: {
        oldPassword: [
          { validator: validateOldPass, trigger: "blur" },
          {
            min: 6,
            max: 16,
            message: "长度至少6位,最长16位",
            trigger: "blur",
          },
        ],
        newPassword: [
          {
            min: 6,
            max: 16,
            message: "长度至少6位,最长16位",
            trigger: "blur",
          },
        ],
        checkPass: [{ validator: validateCheckPass, trigger: "blur" }],
      },
    };
  },
  computed: {
    UserInfo() {
      return this.$store.getters["user/userInfo"];
    },
  },
  created() {
    this.form.id = this.UserInfo.id;
    this.form.name = this.UserInfo.name;
    this.imageUrl = this.UserInfo.avatar;
  },
  methods: {
    cancel() {
      this.$emit("update:visible", false);
    },
    submit() {
      this.$refs.form.validate(async (valid) => {
        console.log("valid", valid);
        if (valid) {
          let res = await this.$store.dispatch("user/update", this.form);
        } else {
          return false;
        }
      });
    },
    avatarChange(e) {
      console.log("上传照片", e);
      let file = e.raw,
        types = ["image/jpeg", "image/jpg", "image/png", "image/gif"],
        isType = types.includes(file.type),
        isLt2M = file.size / 1024 / 1024 < 2;
      if (!isType) {
        this.$message.error("上传头像图片只能是jpeg,jpg,png,gif格式");
        return;
      }
      if (!isLt2M) {
        this.$message.error("上传头像图片大小不能超过 2MB!");
        return;
      }
      this.imageUrl = URL.createObjectURL(file);
      console.log("imageUrl", this.imageUrl);
      this.form.avatar = file;
    },
  },
};
</script>

<style lang="scss" scoped>
.avatar-uploader {
  margin-bottom: 18px;
  text-align: center;
  .avatarWrpper {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 128px;
    height: 128px;
    border-radius: 50%;
    // border:1px solid black;
    border: 1px solid white;
    .avatar {
      width: 100%;
      height: 100%;
      border-radius: 50%;
    }
    .icon {
      font-size: 32px;
    }
  }
}

.buttonWrapper{
  text-align: center;
}
/deep/.el-form-item__label {
	text-align-last: justify;
}
</style>
