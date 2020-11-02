<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/Home' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>用户管理</el-breadcrumb-item>
      <el-breadcrumb-item>登陆信息记录</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card>
      Welcome to EvanWebFrontEnd !!! The acessToken time out is 20
      minutes</el-card
    >

    <div class="Data">
      <el-table :data="ListUsersInfo" border style="width: 100%">
        <el-table-column type="index" label="#" width="100"> </el-table-column>
        <el-table-column
          prop="privacyUrl"
          label="获取登陆信息(仅管理员查看)"
          width="180"
        >
        </el-table-column>
        <el-table-column label="是否获取认证" width="180">
          <template v-slot="scope">
            <el-switch
              v-model="scope.row.isAuthenticated"
              active-color="#13ce66"
              inactive-color="#ff4949"
              disabled
            ></el-switch>
          </template>
        </el-table-column>
        <el-table-column prop="authenticationType" label="认证类型">
        </el-table-column>
        <el-table-column prop="username" label="用户名"> </el-table-column>
        <el-table-column prop="role" label="角色"> </el-table-column>
        <el-table-column
          prop="dateTimeStart"
          label="登陆时间"
        ></el-table-column>
        <el-table-column prop="refreshToken" label="刷新Token">
        </el-table-column>
      </el-table>

      <!-- 分页部分 -->
      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page="queryInfo.pageindex"
        :page-sizes="[10, 15, 20]"
        :page-size="queryInfo.pageSize"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      >
      </el-pagination>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      queryInfo: {
        oderyFont: "DateTimeStart desc",
        pageSize: 10,
        pageindex: 1,
      },
      ListUsersInfo: [],
      total: 0,
    };
  },
  created() {
    this.GetCompaniesList();
  },
  methods: {
    GetCompaniesList() {
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
          if (res.status != 200) {
            return this.$message.error("查询失败");
          }
          console.log("res", res);
          res.data.data.forEach((item) => {
            item.dateTimeStart = item.dateTimeStart.replace("T", " ");
          });
          this.ListUsersInfo = res.data.data;
          this.total = res.data.total;
          console.log("ListUsersInfo数据", this.ListUsersInfo);
        })
        .catch((err) => {
          if (err.message.indexOf("401") !== -1) {
            return this.$message.error("accessToken不正确或已过期,请重新登陆");
          }
          return this.$message.error("查询登陆信息失败,请查看服务器是否开启");
        });
    },

    // 监听pageSize的改变事件
    handleSizeChange(newSize) {
      this.queryInfo.pageSize = newSize;
      this.GetCompaniesList();
    },
    // 监听pageindex的改变事件
    handleCurrentChange(newpage) {
      this.queryInfo.pageindex = newpage;
      this.GetCompaniesList();
    },
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