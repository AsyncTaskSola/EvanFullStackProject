<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/Home' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>用户管理</el-breadcrumb-item>
      <el-breadcrumb-item>登陆信息记录</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card class="box-card"> 123 </el-card>

    <div class="Data">
      <el-table :data="tableData" border style="width: 100%">
        <el-table-column prop="date" label="日期" width="180">
        </el-table-column>
        <el-table-column prop="name" label="姓名" width="180">
        </el-table-column>
        <el-table-column prop="address" label="地址"> </el-table-column>
      </el-table>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      ListUsersInfo: [],
    };
  },
  created() {
    // this.$http
    //   .get("/api/LoginUserInfo/GetLoginInfo", {
    //     headers: {
    //       Authorization: sessionStorage.getItem("Authorization"),
    //     },
    //   })
    //   .then((res) => {
    //     console.log("登陆信息记录", res);
    //   });
  },
};
</script>

<style style="less" scoped>
.Data {
  margin-top: 30px;
}
</style>