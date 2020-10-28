<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/Home' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>公司管理</el-breadcrumb-item>
      <el-breadcrumb-item>公司信息</el-breadcrumb-item>
    </el-breadcrumb>

    <!-- 卡片视图 -->
    <el-card class="box-card">
      <el-row :gutter="10">
        <el-col :span="7">
          <el-input placeholder="请输入公司名称" clearable v-model="queryInfo.querycompanyName" @clear="GetCompaniesList">
            <el-button
              slot="append"
              icon="el-icon-search"
              @click="GetCompaniesList"
            ></el-button> </el-input
        ></el-col>
        <el-col :span="4">
          <el-button type="primary">添加公司</el-button>
        </el-col>
      </el-row>

      <!-- 表格区域 -->
      <div class="Data">
        <el-table :data="ListCompaniesInfo" border>
          <el-table-column type="index" label="#"> </el-table-column>
          <el-table-column prop="id" label="GUID" width="300">
          </el-table-column>
          <el-table-column prop="name" label="公司名" width="180">
          </el-table-column>
          <el-table-column prop="introduction" label="公司描述">
          </el-table-column>

          <el-table-column label="操作">
            <!-- 作用与插槽 -->
            <template v-solt="scoped">
              <el-tooltip effect="dark" content="编辑" placement="top" :hide-after="0" > 
                <el-button type="warning " icon="el-icon-edit"></el-button>
              </el-tooltip>

              <el-tooltip effect="dark" content="删除" placement="top" :hide-after="0" >
                <el-button type="danger" icon="el-icon-delete"></el-button>
              </el-tooltip>
            </template>
          </el-table-column>
        </el-table>
      </div>

      <!-- 分页区域 -->
      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page="queryInfo.pageindex"
        :page-sizes="[1, 2, 5, 10]"
        :page-size="queryInfo.pageSize"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      >
      </el-pagination>
    </el-card>
  </div>
</template>

<script>
export default {
  data() {
    return {
      queryInfo: {
        querycompanyName:'',  
        pageSize: 2,
        pageindex: 1,
      },
      ListCompaniesInfo: [],
      total: 0,
    };
  },
  methods: {
    GetCompaniesList() {
      this.$http
        .get(
          `/api/CEGC/Companies/GetCompanies?pageSize=${this.queryInfo.pageSize}&pageindex=${this.queryInfo.pageindex}&querycompanyName=${this.queryInfo.querycompanyName}`,
          {
            headers: {
              Authorization: sessionStorage.getItem("Authorization"),
            },
          }
        )
        .then((res) => {
          if (res.status != 200) {
            return this.$message.error("查询公司信息失败");
          }
          console.log("公司数据", res);
          this.ListCompaniesInfo = res.data.data;
          this.total = res.data.total;
        })
        .catch((err) => {
          if (err.message.indexOf("401") !== -1) {
            return this.$message.error("accessToken不正确或已过期,请重新登陆");
          }
          return this.$message.error("查询公司信息失败,请查看服务器是否开启");
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
  created() {
    this.GetCompaniesList();
  },
};
</script>

<style style="less" scoped>
.Data {
  margin-top: 30px;
}
</style>