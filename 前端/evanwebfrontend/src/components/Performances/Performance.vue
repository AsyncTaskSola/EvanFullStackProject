<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/Home' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>业绩管理</el-breadcrumb-item>
      <el-breadcrumb-item>业绩看板</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card>
      <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
      <div id="main" style="width: 900px; height: 600px"></div>
    </el-card>
  </div>
</template>

<script>
import echarts from "echarts";
export default {
  data() {
    return {
      queryInfo: {
        queryEmployeeName: "",
        oderyFont: "dateofBirth desc",
        pageSize: 1000,
        pageindex: 1,
      },
      EmployeeInfoList: [],
      EmployeeName: [],
      EmoloyeePerformance: [],
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
          this.EmployeeInfoList = res.data.data;
          this.total = res.data.total;
          this.ListAssembly();
          this.init();
        });
    },
    ListAssembly() {
      this.EmployeeInfoList.forEach((x) => {
        this.EmployeeName.push(x.firstName);
        this.EmoloyeePerformance.push(x.performance);
      });
      console.log(this.EmployeeName);
      console.log(this.EmoloyeePerformance);
    },
    init() {
      // 基于准备好的dom，初始化echarts实例
      var myChart = echarts.init(document.getElementById("main"));
      var option = {
        title: {
          text: "员工业绩看板",
        },
        tooltip: {},
        legend: {
          data: ["个人业绩"],
        },
        xAxis: {
          data: this.EmployeeName,
          axisLabel: {
            color: "#f70606",
          },
        },
        yAxis: {},
        series: [
          {
            name: "个人业绩",
            type: "bar",
            data: this.EmoloyeePerformance,
            itemStyle: {
              color: "#ebb563",
              //   opacity: 0.7,
              //   shadowColor: "rgba(65, 153, 180, 0.8)",
              //   shadowBlur: 10,
            },
          },
        ],
      };
      myChart.setOption(option);
    },
  },
  mounted() {
    this.GetEmployeeList();

    //console.log("数据测试",this.EmoloyeePerformance);
  },
};
</script>

<style>
</style>