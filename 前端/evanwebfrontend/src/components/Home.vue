<template>
  <el-container>
    <!-- 头部区域 -->
    <el-header>
      <div>
        <span>EvanWebFrontEnd</span>
      </div>
      <el-button type="warning" @click="Logout">退出</el-button></el-header
    >
    <!-- 页面主体区 -->
    <el-container>
      <!-- 侧边栏 -->
      <div class="menuWrapper">
        <div class="toggle-button" @click="toggleCollapse">折叠区</div>
        <el-menu
          :default-active="$route.path"
          class="el-menu-vertical-demo"
          background-color="#545c64"
          text-color="#fff"
          active-text-color="#ffd04b"
          :unique-opened="true"
          :collapse="Iscollapse"
          :router="true"
        >
          <el-submenu index="1">
            <!-- 一级菜单 -->
            <template slot="title">
              <i class="el-icon-s-custom"></i>
              <span>用户管理</span>
            </template>
            <!-- 二级菜单 -->
            <el-menu-item index="/Users">
              <template slot="title">
                <i class="el-icon-s-grid"></i>
                <span>当前登陆用户</span>
              </template>
            </el-menu-item>

             <el-menu-item index="/UsersInfo">
              <template slot="title">
                <i class="el-icon-s-grid"></i>
                <span>登陆信息记录</span>
              </template>
            </el-menu-item>
          </el-submenu>

          <el-submenu index="2">
            <!-- 一级菜单 -->
            <template slot="title">
              <i class="el-icon-office-building"></i>
              <span>公司管理</span>
            </template>
            <!-- 二级菜单 -->
            <el-menu-item index="/CompaniesInfo">
              <template slot="title">
                <i class="el-icon-s-grid"></i>
                <span>公司信息</span>
              </template>
            </el-menu-item>
          </el-submenu>

          <el-submenu index="3">
            <!-- 一级菜单 -->
            <template slot="title">
              <i class="el-icon-s-check"></i>
              <span>员工管理</span>
            </template>
            <!-- 二级菜单 -->
            <el-menu-item index="/EmployeesInfo">
              <template slot="title">
                <i class="el-icon-s-grid"></i>
                <span>员工信息</span>
              </template>
            </el-menu-item>
          </el-submenu>

          <el-submenu index="4">
            <!-- 一级菜单 -->
            <template slot="title">
              <i class="el-icon-aim"></i>
              <span>管理员区</span>
            </template>
            <!-- 二级菜单 -->
            <el-menu-item index="/AdminArea">
              <template slot="title">
                <i class="el-icon-s-grid"></i>
                <span>公司员工信息汇总</span>
              </template>
            </el-menu-item>
          </el-submenu>

          <el-submenu index="5">
            <!-- 一级菜单 -->
            <template slot="title">
              <i class="el-icon-s-data"></i>
              <span>业绩管理</span>
            </template>
            <!-- 二级菜单 -->
            <el-menu-item index="/Performance">
              <template slot="title">
                <i class="el-icon-data-analysis"></i>
                <span>业绩看板</span>
              </template>
            </el-menu-item>
          </el-submenu>
        </el-menu>
      </div>
      <!-- 右侧内容主体 -->
      <el-main>
        <router-view></router-view>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
export default {
  data() {
    return {
      Iscollapse: false,
      LoginUserInfo: {},
    };
  },
  methods: {
    Logout() {
      sessionStorage.clear();
      this.$message.success("退出成功,正在为你跳转到首页");
      this.$router.push("/Login");
    },
    // 点击按钮切换菜单的折叠
    toggleCollapse() {
      this.Iscollapse = !this.Iscollapse;
    },
  },
  created() {
  },
};
</script>

<style lang="less" scoped>
.el-header {
  background-color: #739bb7;
  display: flex;
  justify-content: space-between; //左右对齐
  padding-left: 20px;
  align-items: center;
  color: #fff;
  font-size: 20px;
}
.el-aside {
  background-color: #545c64;
  .el-menu {
    border-right: none;
  }
}
.menuWrapper {
  display: flex;
  flex-direction: column;
}

.iconfont {
  margin-right: 10px;
}

.toggle-button {
  background-color: #545c64;
  font-size: 15px;
  line-height: 25px;
  color: white;
  text-align: center;
  letter-spacing: 0.2em;
  cursor: pointer;
}

/deep/.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 200px;
  min-height: 400px;
}

.el-menu-vertical-demo {
  flex: 1;
  border-right: 0;
}
.el-main{
  padding: 8px;;
}
</style>