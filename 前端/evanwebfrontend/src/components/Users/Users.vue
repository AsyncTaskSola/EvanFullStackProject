<template>
  <div class="UserArea">
    <h3>当前用户区域信息</h3>
    <div class="UserInfo">
      <h4>
        当前登陆用户：<span>{{ this.LoginUserInfo.username }}</span>
      </h4>
      <h4>
        Role:<span>{{ this.LoginUserInfo.role }}</span>
      </h4>
      <h4>
        获取登陆具体信息(仅管理员):<span>{{
          this.LoginUserInfo.privacyUrl
        }}</span>
      </h4>
      <!-- <h4>AccessToken：{{this.LoginUserInfo.accessToken}}</h4>      -->
      <h4>
        DateTimeStart:<span>{{
          this.LoginUserInfo.dateTimeStart.replace("T", " ")
        }}</span>
      </h4>
      <h4>
        RefreshToken:<span>{{ this.LoginUserInfo.refreshToken }}</span>
      </h4>
    </div>

    <div class="Technology">
      <h3>涉及技术</h3>
      <section>
        <div>
          <Tag>HTML</Tag>
        </div>
        <div>
          <Tag>CSS3</Tag>
        </div>
        <div>
          <Tag>JavaScript</Tag>
        </div>
        <div>
          <Tag>.NET Core</Tag>
        </div>
        <div>
          <Tag>Vue.js</Tag>
        </div>
        <div>
          <Tag>IDS4</Tag>
        </div>
        <div>
          <Tag>Log4</Tag>
        </div>
        <div>
          <Tag>SqlSugar</Tag>
        </div>
        <div>
          <Tag>Swagger</Tag>
        </div>
        <div>
          <Tag>Es6</Tag>
        </div>
        <div>
          <Tag>Element</Tag>
        </div>
        <div>
          <Tag>autoFace</Tag>
        </div>
      </section>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      LoginUserInfo: {},
    };
  },
  methods: {},
  created() {
    this.$http
      .post(
        "/api/LoginUserInfo/GetInfo",
        {},
        {
          headers: {
            Authorization: sessionStorage.getItem("Authorization"),
          },
        }
      )
      .then((res) => {
        if (res.status != 200) {
          return this.$message.error("获取当前用户信息失败");
        }
        this.LoginUserInfo = res.data.data;
        console.log("当前用户区域", this.LoginUserInfo);
      })
      .catch((err) => {
        if (err.message.indexOf("401") !== -1) {
          return this.$message.error("accessToken不正确或已过期,请重新登陆");
        }
        return this.$message.error("查询公司信息失败,请查看服务器是否开启");
      });
  },
};
</script>

<style lang="less" scoped>
.UserArea {
  width: 600px;
  height: 300px;
  background-color: antiquewhite;
  border-radius: 5%;
  padding: 5px;
  box-shadow: 0 0 3px black;
  margin: 0 auto;
  h3 {
    width: 100%;
    height: 10px;
    line-height: 10px;
    display: flex;
    justify-content: center;
    align-content: center;
    color: coral;
  }
}
.UserInfo {
  overflow: auto;
}
span {
  color: green;
  padding: 10px;
}

.Technology {
  perspective: 900px;
}
.Technology section {
  position: relative;
  width: 100px;
  height: 75px;
  margin: 80px auto;
  transform-style: preserve-3d;
  border-radius: 20%;
  /* 添加动画效果 */
  animation: rotate 60s linear infinite;
}
@keyframes rotate {
  0% {
  }
  100% {
    transform: rotateY(360deg);
  }
}

.Technology section div {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgb(238, 209, 171);
  display: flex;
  justify-content: center;
  align-items: center;
}

.Technology section div tag {
  font-size: 20px;
}
.Technology section div:nth-of-type(1) {
  transform: translateZ(300px);
}

.Technology section div:nth-of-type(2) {
  transform: rotateY(30deg) translateZ(300px);
}

.Technology section div:nth-of-type(3) {
  transform: rotateY(60deg) translateZ(300px);
}

.Technology section div:nth-of-type(4) {
  transform: rotateY(90deg) translateZ(300px);
}

.Technology section div:nth-of-type(5) {
  transform: rotateY(120deg) translateZ(300px);
}

.Technology section div:nth-of-type(6) {
  transform: rotateY(150deg) translateZ(300px);
}
.Technology section div:nth-of-type(7) {
  transform: rotateY(180deg) translateZ(300px);
}

.Technology section div:nth-of-type(8) {
  transform: rotateY(210deg) translateZ(300px);
}

.Technology section div:nth-of-type(9) {
  transform: rotateY(240deg) translateZ(300px);
}

.Technology section div:nth-of-type(10) {
  transform: rotateY(270deg) translateZ(300px);
}

.Technology section div:nth-of-type(11) {
  transform: rotateY(300deg) translateZ(300px);
}
.Technology section div:nth-of-type(12) {
  transform: rotateY(330deg) translateZ(300px);
}
</style>