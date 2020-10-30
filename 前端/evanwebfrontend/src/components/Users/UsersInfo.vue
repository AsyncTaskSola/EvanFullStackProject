<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/Home' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>用户管理</el-breadcrumb-item>
      <el-breadcrumb-item>登陆信息记录</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card> Welcome to EvanWebFrontEnd !!!</el-card>

    <div class="Data">
      <el-table :data="tableData" border style="width: 100%">
        <el-table-column type="index" label="#" width="180"> </el-table-column>
        <el-table-column
          prop="date"
          label="获取登陆信息(仅管理员查看)"
          width="180"
        >
        </el-table-column>
        <el-table-column prop="name" label="是否获取认证" width="180">
        </el-table-column>
        <el-table-column prop="address" label="认证类型"> </el-table-column>
        <el-table-column prop="address" label="用户名"> </el-table-column>
        <el-table-column prop="address" label="角色"> </el-table-column>
        <el-table-column prop="address" label="登陆时间"> </el-table-column>
      </el-table>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      queryInfo: {
        oderyFont: "DateTimeStart desc",
        pageSize: 5,
        pageindex: 1,
      },
      ListUsersInfo: [],
      total: 0,
    };
  },
  created() {
    this.$http
      .get(
        `/api/LoginUserInfo/GetLoginInfo?pageSize=${this.queryInfo.pageSize}&pageindex=${this.queryInfo.pageindex}&oderyFont=${this.queryInfo.oderyFont}`,
        {
          headers: {
            Authorization: sessionStorage.getItem("Authorization"),
          },
        }
      )
      .then((res) => {       
        
        this.ListUsersInfo=res.data.data;
        console.log("ListUsersInfo数据",this.ListUsersInfo);
      })
      .catch((err) => {
        if (err.message.indexOf("401") !== -1) {
          return this.$message.error("accessToken不正确或已过期,请重新登陆");
        }
        return this.$message.error("查询登陆信息失败,请查看服务器是否开启");
      });
  },
};
</script>

<style style="less" scoped>
.Data {
  margin-top: 30px;
}
.el-card {
  font-size: 20px;
  color: orangered;
  background-color: antiquewhite;
}
/* class="box-card" */
</style>