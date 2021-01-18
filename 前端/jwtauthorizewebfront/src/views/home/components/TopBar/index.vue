<template>
  <div class="topBar">
    <div class="pageInfo">
      <div class="info">
        <MenuCtrl />
        <Bread />
      </div>
      <div class="labelWrapper"></div>
    </div>
    <div class="userInfo">
      <UserInfo/>
    </div>
  </div>
</template>

<script>
import MenuCtrl from "./components/MenuCtrl/index";
import Bread from "./components/Bread/index";
import UserInfo from "./components/UserInfo/index";
export default {
  name: "TopBar",
  components: {
    MenuCtrl,
    Bread,
    UserInfo
  },
};
</script>

<style lang="scss" scoped>
.topBar {
  position: relative;
  padding: 5px 10px;
  display: flex;
  align-items: center;
  &:after {
    content: "";
    position: absolute;
    right: 0;
    bottom: 0;
    left: 0;
    border-bottom: 1px solid black;
    transform: scaleY(0.5);
  }
}
.pageInfo{
  display: flex;
  flex:1;//让图片在右边的，兄弟组件
  flex-direction: column;
  .info{
    display: flex;
    align-content: center;
  }
}
</style>