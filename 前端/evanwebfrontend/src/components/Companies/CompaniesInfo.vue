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
          <el-input
            placeholder="请输入公司名称"
            clearable
            v-model="queryInfo.querycompanyName"
            @clear="GetCompaniesList"
          >
            <el-button
              slot="append"
              icon="el-icon-search"
              @click="GetCompaniesList"
            ></el-button> </el-input
        ></el-col>
        <el-col :span="4">
          <el-button type="primary" @click="AddCompany">添加公司</el-button>
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
          <el-table-column prop="companyEmail" label="公司邮箱">
          </el-table-column>
          <el-table-column prop="companyPhone" label="公司联系方式">
          </el-table-column>
          <!-- <el-table-column prop="currentTime" label="创建时间">
          </el-table-column> -->

          <el-table-column label="操作">
            <!-- 作用与插槽 -->
            <template v-slot="scope">
              <el-tooltip
                effect="dark"
                content="编辑"
                placement="top"
                :hide-after="0"
              >
                <el-button
                  type="warning "
                  icon="el-icon-edit"
                  size="mini"
                  @click="EditCompany(scope.row.id)"
                ></el-button>
              </el-tooltip>

              <el-tooltip
                effect="dark"
                content="删除"
                placement="top"
                :hide-after="0"
              >
                <el-button
                  type="danger"
                  icon="el-icon-delete"
                  size="mini"
                  @click="DeleteCompany(scope.row.id)"
                ></el-button>
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
        :page-sizes="[3, 5, 10]"
        :page-size="queryInfo.pageSize"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      >
      </el-pagination>
    </el-card>

    <!-- Dialog 添加对话框 -->
    <el-dialog
      title="添加公司"
      :visible.sync="AddDialogVisible"
      width="40%"
      @close="AddDialogClose"
    >
      <el-form
        :model="AddForm"
        :rules="AddRules"
        ref="AddRef"
        label-width="70px"
        class="demo-ruleForm"
      >
        <el-form-item label="Guid" prop="id">
          <el-input v-model="AddForm.id" :disabled="true"></el-input>
        </el-form-item>
        <el-form-item label="创建(编辑)时间" prop="currenttime">
          <el-input v-model="AddForm.currenttime" :disabled="true"></el-input>
        </el-form-item>
        <el-form-item label="公司名" prop="name">
          <el-input v-model="AddForm.name"></el-input>
        </el-form-item>
        <el-form-item label="公司描述" prop="introduction">
          <el-input v-model="AddForm.introduction"></el-input>
        </el-form-item>
        <el-form-item label="公司邮箱" prop="companyemail">
          <el-input v-model="AddForm.companyemail"></el-input>
        </el-form-item>
        <el-form-item label="公司联系方式" prop="companyphone">
          <el-input v-model="AddForm.companyphone"></el-input>
        </el-form-item>
      </el-form>

      <span slot="footer" class="dialog-footer">
        <el-button @click="AddDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="AddFormData()">确 定</el-button>
      </span>
    </el-dialog>

    <!-- Dialog 修改对话框 -->
    <el-dialog
      title="编辑公司"
      :visible.sync="EditDialogVisible"
      width="40%"
      v-dialogDrag
    >
      <el-form
        :model="EditForm"
        :rules="EditRules"
        ref="EditRef"
        label-width="70px"
        class="demo-ruleForm"
      >
        <!-- <el-form-item label="Guid" prop="id">
          <el-input v-model="EditForm.id" :disabled="true"></el-input>
        </el-form-item>
        <el-form-item label="创建(编辑)时间" prop="currenttime">
          <el-input v-model="EditForm.currentTime" :disabled="true"></el-input>
        </el-form-item> -->
        <el-form-item label="公司名">
          <el-input v-model="EditForm.name" :disabled="true"></el-input>
        </el-form-item>
        <el-form-item label="公司描述">
          <el-input v-model="EditForm.introduction"></el-input>
        </el-form-item>
        <el-form-item label="公司邮箱" prop="companyEmail">
          <el-input v-model="EditForm.companyEmail"></el-input>
        </el-form-item>
        <el-form-item label="公司联系方式" prop="companyPhone">
          <el-input v-model="EditForm.companyPhone"></el-input>
        </el-form-item>
      </el-form>

      <span slot="footer" class="dialog-footer">
        <el-button @click="EditDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="EditFormData()">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
export default {
  data() {
    var checkemail = (rule, value, cb) => {
      if (!value) return cb();
      const regemail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      if (regemail.test(value)) {
        return cb();
      }
      cb(new Error("请输入合法邮箱"));
    };
    var checkmobile = (rule, value, cb) => {
      if (!value) return cb();
      const regmobile = /^1[3456789]\d{9}$/;
      if (regmobile.test(value)) {
        return cb();
      }
      cb(new Error("请输入合法手机号"));
    };

    return {
      queryInfo: {
        querycompanyName: "",
        oderyFont: "CurrentTime desc",
        pageSize: 5,
        pageindex: 1,
      },
      ListCompaniesInfo: [],
      total: 0,

      //控制添加弹框显示和隐藏
      AddDialogVisible: false,
      //添加表单的数据
      AddForm: {
        id: "",
        name: "",
        introduction: "",
        currenttime: "",
        companyemail: "",
        companyphone: "",
        emplyees: [],
      },
      //添加的规则
      AddRules: {
        name: [
          { required: true, message: "请输入公司名称", trigger: "blur" },
          {
            min: 3,
            max: 10,
            message: "长度在 3 到 10 个字符",
            trigger: "blur",
          },
        ],
        companyemail: [
          {
            validator: checkemail,
            trigger: "blur",
          },
        ],
        companyphone: [
          {
            validator: checkmobile,
            trigger: "blur",
          },
        ],
      },
      //-------------------------------------------------------------------------------------------------------------
      //控制编辑弹框显示和隐藏
      EditDialogVisible: false,
      //编辑表单的数据
      EditForm: {},
      //编辑的规则
      EditRules: {
        companyEmail: [
          {
            validator: checkemail,
            trigger: "blur",
          },
        ],
        companyPhone: [
          {
            validator: checkmobile,
            trigger: "blur",
          },
        ],
      },
    };
  },
  methods: {
    GetCompaniesList() {
      this.$http
        .get(
          `/api/CEGC/Companies/GetCompanies?pageSize=${this.queryInfo.pageSize}&pageindex=${this.queryInfo.pageindex}&querycompanyName=${this.queryInfo.querycompanyName}&oderyFont=${this.queryInfo.oderyFont}`,
          {
            headers: {
              Authorization: sessionStorage.getItem("Authorization"),
            },
          }
        )
        .then((res) => {
          //   if (res.status != 200) {
          //     return this.$message.error("查询公司信息失败");
          //   }
          console.log("公司数据", res);
          //   this.$message.success("查询公司信息成功");
          this.ListCompaniesInfo = res.data.data;
          this.total = res.data.total;
        });
      // .catch((err) => {
      //   if (err.message.indexOf("401") !== -1) {
      //     return this.$message.error("accessToken不正确或已过期,请重新登陆");
      //   }
      //   return this.$message.error("查询公司信息失败,请查看服务器是否开启");
      // });
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
    AddCompany() {
      this.AddDialogVisible = true;
      var curguid = "";
      this.AddForm.id = this.$guid();
      this.AddForm.currenttime = this.$Gettime();
    },
    //添加公司关闭事件，清空数据
    AddDialogClose() {
      this.$refs.AddRef.resetFields();
    },
    //添加表单数据
    AddFormData() {
      //先表单数据验证
      this.$refs.AddRef.validate((valid) => {
        if (!valid) return;
        let form = JSON.parse(JSON.stringify(this.AddForm));
        form.currenttime = form.currenttime.replace(" ", "T");
        this.$http
          .post("/api/CEGC/Companies/Add", [form], {
            headers: {
              Authorization: sessionStorage.getItem("Authorization"),
            },
          })
          //    multipart/form-data
          .then((res) => {
            // if (res.status != 200) {
            //   return this.$message.error("添加失败");
            // }
            // this.$message.success("添加成功");
            this.AddDialogVisible = false;
            this.GetCompaniesList();
          });
        //   .catch((err) => {
        //     if (err.message.indexOf("401") !== -1) {
        //       return this.$message.error(
        //         "accessToken不正确或已过期,请重新登陆"
        //       );
        //     }
        //     return this.$message.error("添加失败");
        //   });
      });
    },
    //---------------------------------------------------------------------------
    //编辑查看当前的数据
    EditCompany(id) {
      console.log(id);
      this.$http
        .get(`/api/CEGC/Companies/UpdateId?companyId=${id}`, {
          headers: {
            Authorization: sessionStorage.getItem("Authorization"),
          },
        })
        .then((res) => {
          if (res.status != 200) {
            return this.$message.error("查询当前数据失败");
          }
          this.EditForm = res.data.data;
          this.EditForm.currentTime = this.$Gettime();
        })
        .catch((err) => {
          if (err.message.indexOf("401") !== -1) {
            return this.$message.error("accessToken不正确或已过期,请重新登陆");
          }
          return this.$message.error("查询当前数据失败");
        });
      this.EditDialogVisible = true;
    },
    //编辑表单数据
    EditFormData() {
      this.$refs.EditRef.validate((valid) => {
        if (!valid) return;
        console.log("更新数据", this.EditForm);
        let editform = JSON.parse(JSON.stringify(this.EditForm));
        editform.currentTime = editform.currentTime.replace(" ", "T");
        editform.emplyees = [];
        this.$http
          .post("/api/CEGC/Companies/Update", editform, {
            headers: {
              Authorization: sessionStorage.getItem("Authorization"),
            },
          })
          .then((res) => {
            // if (res.status != 200) {
            //   return this.$message.error("修改失败");
            // }
            // this.$message.success("修改成功");
            this.EditDialogVisible = false;
            this.GetCompaniesList();
          });
        //   .catch((err) => {
        //     if (err.message.indexOf("401") !== -1) {
        //       return this.$message.error(
        //         "accessToken不正确或已过期,请重新登陆"
        //       );
        //     }
        //     return this.$message.error("修改失败");
        //   });
      });
    },
    //删除数据
    DeleteCompany(CompanyIds) {
      //  询问用户是否删除
      this.$confirm("此操作将永久删除该数据, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then((res) => {
          if (res === "confirm") {
            console.log("删除", CompanyIds);
            this.$http
              .post("/api/CEGC/Companies/Delete", [CompanyIds], {
                headers: {
                  Authorization: sessionStorage.getItem("Authorization"),
                },
              })
              .then((res) => {
                // if (res.status != 200) {
                //   return this.$message.error("删除失败");
                // }
                // this.$message.success("删除成功");
                this.EditDialogVisible = false;
                this.GetCompaniesList();
              });
            //   .catch((err) => {
            //     if (err.message.indexOf("401") !== -1) {
            //       return this.$message.error(
            //         "accessToken不正确或已过期,请重新登陆"
            //       );
            //     }
            //     return this.$message.error("修改失败");
            //   });
          }
        })
        .catch((res) => res);
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

/deep/.el-input.is-disabled .el-input__inner {
  background-color: antiquewhite;
  border-color: white;
  color: black;
  cursor: not-allowed;
}
</style>