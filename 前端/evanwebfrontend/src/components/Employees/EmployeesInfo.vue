<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/Home' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>员工管理</el-breadcrumb-item>
      <el-breadcrumb-item>员工信息</el-breadcrumb-item>
    </el-breadcrumb>

    <!-- 卡片视图 -->
    <el-card class="box-card">
      <el-row :gutter="10">
        <el-col :span="7">
          <el-input
            placeholder="请输入员工名称"
            clearable
            v-model="queryInfo.queryEmployeeName"
            @clear="GetEmployeeList"
          >
            <el-button
              slot="append"
              icon="el-icon-search"
              @click="GetEmployeeList"
            ></el-button> </el-input
        ></el-col>
        <el-col :span="4">
          <el-button type="primary" @click="AddEmoloyee">添加员工</el-button>
        </el-col>
      </el-row>

      <!-- 表格区域 -->
      <div class="Data">
        <el-table :data="ListEmoloyeeInfo" border>
          <el-table-column type="index" label="#"> </el-table-column>
          <el-table-column prop="companyName" label="所属公司" width="170">
          </el-table-column>
          <el-table-column prop="emplyeeNo" label="员工编码" width="100">
          </el-table-column>
          <el-table-column prop="firstName" label="员工名"> </el-table-column>
          <el-table-column label="性别">
            <template v-slot="scope">
              <!-- {{scope.row.companyId}} -->
              {{ scope.row.gender == 1 ? "男" : "女" }}
            </template>
          </el-table-column>
          <el-table-column prop="dateofBirth" label="生日"> </el-table-column>
          <el-table-column prop="performance" label="个人业绩">
          </el-table-column>
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
                  @click="EditEmoloyee(scope.row.id)"
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
                  @click="DeleteEmoloyee(scope.row.id)"
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
      title="添加员工"
      :visible.sync="AddDialogVisible"
      width="40%"
      @close="AddDialogClose"
    >
      <el-form
        :model="AddEmployeeForm"
        :rules="AddEmployeeRules"
        ref="AddEmployeeRef"
        label-width="100px"
        class="demo-ruleForm"
      >
        <el-form-item label="员工ID" prop="id">
          <el-input v-model="AddEmployeeForm.id" :disabled="true"></el-input>
        </el-form-item>
        <el-form-item label="所属公司" prop="companyName">
          <el-input v-model="AddEmployeeForm.companyName"></el-input>
        </el-form-item>
        <el-form-item label="员工名" prop="firstName">
          <el-input v-model="AddEmployeeForm.firstName"></el-input>
        </el-form-item>
        <el-form-item label="员工编号" prop="emplyeeNo">
          <el-input v-model="AddEmployeeForm.emplyeeNo"></el-input>
        </el-form-item>
        <el-form-item label="性别" prop="gender">
          <el-select v-model="AddEmployeeForm.gender" placeholder="请选择性别">
            <el-option label="男" value="1"></el-option>
            <el-option label="女" value="2"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="生日" prop="dateofBirth">
          <el-date-picker
            v-model="AddEmployeeForm.dateofBirth"
            type="datetime"
            placeholder="选择日期时间"
            size="small"
            format="yyyy 年 MM 月 dd 日 HH 时 mm 分 ss 秒"
            value-format="yyyy-MM-ddTHH:mm:ss"
          >
          </el-date-picker>
        </el-form-item>
        <el-form-item label="个人业绩" prop="performance">
          <el-input v-model="AddEmployeeForm.performance"></el-input>
        </el-form-item>
      </el-form>

      <span slot="footer" class="dialog-footer">
        <el-button @click="AddDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="AddFormData()">确 定</el-button>
      </span>
    </el-dialog>

    <!-- Dialog 编辑对话框 -->
    <el-dialog
      title="编辑员工"
      :visible.sync="EditDialogVisible"
      width="width"
      v-dialogDrag
    >
      <el-form
        :model="EditEmployeeForm"
        :rules="EditEmployeeRules"
        ref="EditEmployeeRef"
        label-width="100px"
        class="demo-ruleForm"
      >
        <el-form-item label="员工名" prop="firstName">
          <el-input v-model="EditEmployeeForm.firstName"></el-input>
        </el-form-item>
        <el-form-item label="员工编号" prop="emplyeeNo">
          <el-input v-model="EditEmployeeForm.emplyeeNo"></el-input>
        </el-form-item>
        <el-form-item label="性别" prop="gender">
          <el-select v-model="EditEmployeeForm.gender" placeholder="请选择性别">
            <el-option label="男" value="1"></el-option>
            <el-option label="女" value="2"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="生日" prop="dateofBirth">
          <el-date-picker
            v-model="EditEmployeeForm.dateofBirth"
            type="datetime"
            placeholder="选择日期时间"
            size="small"
            format="yyyy 年 MM 月 dd 日 HH 时 mm 分 ss 秒"
            value-format="yyyy-MM-ddTHH:mm:ss"
          >
          </el-date-picker>
        </el-form-item>
        <el-form-item label="个人业绩" prop="performance">
          <el-input v-model="EditEmployeeForm.performance"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer">
        <el-button @click="dialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="EditFormData">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
export default {
  data() {
    return {
      queryInfo: {
        queryEmployeeName: "",
        oderyFont: "dateofBirth desc",
        pageSize: 3,
        pageindex: 1,
      },
      ListEmoloyeeInfo: [],
      total: 0,
      //   添加对话框
      AddDialogVisible: false,
      //添加表单实体
      AddEmployeeForm: {
        id: "",
        companyId: "",
        companyName: "",
        emplyeeNo: "",
        firstName: "",
        gender: "",
        dateofBirth: "",
        performance: "",
      },
      //添加约束
      AddEmployeeRules: {
        companyName: [
          {
            required: true,
            message: "请输入该员工所属的公司名",
            trigger: "blur",
          },
        ],
        firstName: [
          {
            required: true,
            message: "请输入名字",
            trigger: "blur",
          },
        ],
        performance: [
          {
            required: true,
            message: "请输入个人业绩",
            trigger: "blur",
          },
        ],
        dateofBirth: [
          {
            required: true,
            message: "请输入日期",
            trigger: "blur",
          },
        ],
        gender: [
          {
            required: true,
            message: "请选择性别",
            trigger: "blur",
          },
        ],
      },
      //----------------------------------------------------
      //编辑实体
      EditEmployeeForm: {},
      //编辑对话框
      EditDialogVisible: false,
      //编辑约束
      EditEmployeeRules: {
        firstName: [
          {
            required: true,
            message: "请输入名字",
            trigger: "blur",
          },
        ],
        performance: [
          {
            required: true,
            message: "请输入个人业绩",
            trigger: "blur",
          },
        ],
        dateofBirth: [
          {
            required: true,
            message: "请输入日期",
            trigger: "blur",
          },
        ],
        gender: [
          {
            required: true,
            message: "请选择性别",
            trigger: "blur",
          },
        ],
      },
    };
  },
  methods: {
    GetEmployeeList() {
      this.$http
        .get(
          `/api/CEGC/Employees/GetEmployees?pageSize=${this.queryInfo.pageSize}&pageindex=${this.queryInfo.pageindex}&queryEmployeeName=${this.queryInfo.queryEmployeeName}&oderyFont=${this.queryInfo.oderyFont}`,
          {
            headers: {
              Authorization: sessionStorage.getItem("Authorization"),
            },
          }
        )
        .then((res) => {
          console.log("员工数据", res);
          res.data.data.forEach((item) => {
            item.dateofBirth = item.dateofBirth.replace("T", " ");
          });
          this.ListEmoloyeeInfo = res.data.data;
          this.total = res.data.total;
        });
    },
    // 监听pageSize的改变事件
    handleSizeChange(newSize) {
      this.queryInfo.pageSize = newSize;
      this.GetEmployeeList();
    },
    // 监听pageindex的改变事件
    handleCurrentChange(newpage) {
      this.queryInfo.pageindex = newpage;
      this.GetEmployeeList();
    },
    //添加员工弹窗控制
    AddEmoloyee() {
      this.AddDialogVisible = true;
      this.AddEmployeeForm.id = this.$guid();
    },
    //添加员工关闭事件，清空数据
    AddDialogClose() {
      this.$refs.AddEmployeeRef.resetFields();
    },
    //添加员工表单数据
    AddFormData() {
      this.$refs.AddEmployeeRef.validate((valid) => {
        if (!valid) return;
        this.AddEmployeeForm.companyId = this.$guid();
        this.AddEmployeeForm.gender = Number(this.AddEmployeeForm.gender);
        this.AddEmployeeForm.performance = Number(
          this.AddEmployeeForm.performance
        );
        const result = this.AddEmployeeForm;
        console.log("表单的数据", result);
        this.$http
          .post("/api/CEGC/Employees/Add", [this.AddEmployeeForm], {
            Authorization: sessionStorage.getItem("Authorization"),
          })
          .then((res) => {
            this.AddDialogVisible = false;
            this.GetEmployeeList();
          });
      });
    },
    //--------------------------------------------------------------------------------------
    //编辑查看当前单据
    EditEmoloyee(id) {
      this.$http
        .get(`/api/CEGC/Employees/UpdateId?employeeid=${id}`, {
          headers: {
            Authorization: sessionStorage.getItem("Authorization"),
          },
        })
        .then((res) => {
          console.log("当前用户", res);

          this.EditEmployeeForm = res.data.data;
          this.EditEmployeeForm.gender =
            this.EditEmployeeForm.gender === 1 ? "男" : "女";
        });
      this.EditDialogVisible = true;
    },
    //编辑表单数据
    EditFormData() {
      this.$refs.EditEmployeeRef.validate((valid) => {
        if (!valid) return;
        console.log("更新数据", this.EditEmployeeForm);
        this.EditEmployeeForm.dateofBirth = this.EditEmployeeForm.dateofBirth.replace(
          " ",
          "T"
        );
        this.EditEmployeeForm.performance = Number(
          this.EditEmployeeForm.performance
        );
        this.AddEmployeeForm.gender = Number(this.EditEmployeeForm.gender);
        this.$http
          .post("/api/CEGC/Employees/Update", this.EditEmployeeForm, {
            headers: {
              Authorization: sessionStorage.getItem("Authorization"),
            },
          })
          .then((res) => {
            this.EditDialogVisible = false;
            this.GetEmployeeList();
          });
      });
    },
    //删除
    DeleteEmoloyee(EmployeeIds) {
      //  询问用户是否删除
      this.$confirm("此操作将永久删除该数据, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      }).then((res) => {
        if (res === "confirm") {
          this.$http
            .post("/api/CEGC/Employees/Delete", [EmployeeIds], {
              headers: {
                Authorization: sessionStorage.getItem("Authorization"),
              },
            })
            .then((res) => {
              this.EditDialogVisible = false;
              this.GetEmployeeList();
            });
        }
      });
    },
  },
  created() {
    this.GetEmployeeList();
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

/deep/.el-dropdown-link {
  cursor: pointer;
  color: #409eff;
}
/deep/.el-icon-arrow-down {
  font-size: 12px;
}

.el-date-editor.el-input,
.el-date-editor.el-input__inner {
  width: 280px;
}
</style>