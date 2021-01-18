<template>
  <div class="roleListWrapper">
    <div class="rolelists">
      <div class="container">
        <!-- <transition
          name="slide-left"
          v-for="(role, i) in roles"
          :key="role.id"
          tag="div"
        >
          <div
            v-if="showRolesList"
            class="role"
            :style="{ transitionDelay: `0.${i}s`, position: 'relative' }"
          >
            <div class="rolechange">
              <div class="containerrole">
               
              </div>
            </div>
          </div>
        </transition> -->
        <transition name="slide-left">
          <div class="rolelist" v-if="showRolesList">
            <div class="title">
              <h4>权限角色编辑</h4>
              <div class="iconWrapper" @click="close">
                <i class="icon el-icon-close"></i>
              </div>
            </div>
            <el-table
              ref="table"
              row-key="id"
              :data="roles"
              highlight-current-row
              @current-change="handleCurrentChange"
              style="width: 100%"
            >
              <el-table-column type="index" width="50"> </el-table-column>
              <el-table-column property="name" label="Name" width="120">
              </el-table-column>
              <el-table-column property="code" label="Code" width="120">
              </el-table-column>
              <el-table-column property="describe" label="明细" width="100">
              </el-table-column>
            </el-table>
          </div>
        </transition>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: ["showRolesList", "showClickRole", "CurrentInfo"],
  data() {
    return {
      select: "",
      currentroleId: "",
      currentuserId: "",
    };
  },
  methods: {
    async handleCurrentChange(val) {
      if (this.CurrentInfo == "") {
        console.log("已选择角色",val);
        this.currentRow = val;
        this.$emit("CheckCurrentRole",val);
      } else {
        this.currentRow = val;
        var newroleid = val.id;
        var Emtity = {
          UserId: this.currentuserId,
          RoleId: newroleid,
        };
        if (val.id != this.currentroleId) {
          await this.$store.dispatch("role/UpdateUrinfo", Emtity);
          await this.$store.commit("user/resetCurrentUser");
          this.$store.dispatch("user/getUsers", { pageindex: 1, pageSize: 10 });
          return this.$store.getters["user/users"];
        }
      }
    },
    close() {
      //this.showRolesList=false;
      this.$emit("showrole", this.showRolesList);
    },
  },
  created() {
    var res = this.roles;
    console.log("res", res);
    if (this.CurrentInfo == "") {
      return;
    }
    var curr = this.CurrentInfo;
    this.currentuserId = curr.id;
    this.currentroleId = curr.roles.map((x) => x.id)[0];
    console.log("cuyrr", this.currentroleId);
  },
  computed: {
    roles() {
      return this.$store.getters["role/roles"];
    },
  },
  watch: {
    async showRolesList(N) {
      if (N) {
        await this.$store.dispatch("role/getRoles", {
          pageindex: 1,
          pageSize: 10,
        });
        this.$nextTick(() => {
          var currentrole = this.roles.filter(
            (x) => x.id === this.currentroleId
          )[0];
          this.$refs.table.setCurrentRow(currentrole);
        });
      }
    },
    showClickRole(o) {},
  },
};
</script>

<style lang="scss" scoped>
.rolelists {
  position: relative;
  width: 450px;
  height: 100%;
  flex: 1;
  margin-left: 20px;
  .container {
    // background-color: #d9f3dd;
    height: 423px;
    display: flex;
    justify-content: center;
    .title {
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      h4 {
        color: purple;
        font-size: 16px;
        text-align: center;
        margin-top: 15px;
        margin-bottom: 10px;
      }
      &:hover {
        .iconWrapper .icon {
          display: block;
        }
      }
      .iconWrapper {
        position: absolute;
        display: flex;
        right: 10px;
        top: 10px;
        justify-content: center;
        align-content: center;
        height: 15px;
        width: 15px;
        background-color: rgb(243, 86, 86);
        border-radius: 50%;
        .icon {
          display: none;
          cursor: pointer;
          font-size: 14px;
        }
      }
    }
    .rolelist {
      position: relative;
      background-color: #e6e6e6;
      border-radius: 20px;
    }
  }
}
</style>