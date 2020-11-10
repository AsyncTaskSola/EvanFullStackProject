<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/Home' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>管理员区</el-breadcrumb-item>
      <el-breadcrumb-item>公司员工信息汇总</el-breadcrumb-item>
    </el-breadcrumb>

    <!-- 卡片视图 -->
    <el-card class="box-card">
      <el-row :gutter="10">
        <el-col :span="7">
          <el-input
            placeholder="请输入公司名字"
            clearable
            v-model="queryInfo.font"
            @clear="GetCompanyEmployeeList"
          >
            <el-button
              slot="append"
              icon="el-icon-search"
              @click="GetCompanyEmployeeList"
            ></el-button> </el-input
        ></el-col>
      </el-row>

      <!-- 表格区域 -->
      <div class="Data">
        <el-table :data="ListCompanyEmployee" border>
          <el-table-column type="index" label="#"> </el-table-column>
          <el-table-column prop="name" label="公司名" width="140">
          </el-table-column>
          <el-table-column prop="introduction" label="公司描述">
          </el-table-column>
          <el-table-column prop="companyEmail" label="公司邮箱">
          </el-table-column>
          <el-table-column prop="companyPhone" label="公司联系方式">
          </el-table-column>

          <el-table-column type="expand" label="员工信息展开" width="130">
            <template slot-scope="props">
              <el-table :data="props.row.emplyees" style="width: 100%">
                <el-table-column type="index" label="#"> </el-table-column>
                <el-table-column
                  prop="emplyeeNo"
                  label="员工"
                ></el-table-column>
                <el-table-column
                  prop="firstName"
                  label="员工名"
                ></el-table-column>
                <el-table-column
                  prop="emplyeeNo"
                  label="员工编号"
                ></el-table-column>
                <el-table-column
                  prop="performance"
                  label="个人业绩"
                ></el-table-column>
                <el-table-column prop="dateofBirth" label="生日">
                  <template v-slot="scope">
                    {{ scope.row.dateofBirth.replace("T", " ") }}
                  </template>
                </el-table-column>
                <el-table-column prop="gender" label="性别">
                  <template v-slot="scope">
                    {{ scope.row.gender == 1 ? "男" : "女" }}
                  </template>
                </el-table-column>
              </el-table>
            </template>
          </el-table-column>
        </el-table>
      </div>

      <!-- 分页区域 -->
      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page="queryInfo.pageindex"
        :page-sizes="[5, 10, 15]"
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
        font: "",
        oderyFont: "CurrentTime desc",
        pageSize: 5,
        pageindex: 1,
      },
      ListCompanyEmployee: [],
      total: 0,
    };
  },
  methods: {
    GetCompanyEmployeeList() {
      this.$http
        .get(
          `/api/CEGC/Companies/GetCompaniesEmployeeInfo?pageSize=${this.queryInfo.pageSize}&pageindex=${this.queryInfo.pageindex}&font=${this.queryInfo.font}&oderyFont=${this.queryInfo.oderyFont}`
        )
        .then((res) => {
          console.log("特别区域", res);
          this.ListCompanyEmployee = res.data.data.data;
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
    this.GetCompanyEmployeeList();
  },
};
</script>

<style lang="less" scope>
</style>