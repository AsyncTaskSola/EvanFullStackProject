<template>
  <div id="users" class="pageContatiner">
    <div class="usersWrapper">
      1
      <div class="usersList">
        2
        <div class="searchWrapper">
          <el-input
            placeholder="请输入内容"
            v-model="searchUser"
            class="input-with-select"
            size="mini"
          >
            <el-button
              slot="append"
              icon="el-icon-search"
              size="mini"
            ></el-button>
          </el-input>
        </div>
        <div class="list">
          <div class="listWrapper">
            <UsersItem
              v-for="user in userData.data"
              :key="user.id"
              :user="user"
              @getUser="getUser"
            ></UsersItem>
          </div>
          <div class="ListFoot">
            <p>共{{ userData.total ? userData.total : 0 }}个用户</p>
            <div class="iconWrapper" @click="addUser">
              <i class="icon el-icon-plus"></i>
            </div>
          </div>
        </div>
      </div>
      <div class="userInfoWrapper">
        6
        <transition name="slide-right">
          <component
            :is="currentComponent"
            :UserInfodata="componentData"
          ></component>
        </transition>
      </div>
    </div>
  </div>
</template>


<script>
import UsersItem from "./components/UserItem/index";
import AddUser from "./components/AddUser/index";
export default {
  name: "users",
  components: {
    UsersItem,
    // AddUser,
    //懒加载
    UserInfo: () =>
      import(/* webpackChunkName: "UserInfo" */ "./components/UserInfo"),
    AddUser: () =>
      import(/* webpackChunkName: "AddUser" */ "./components/AddUser"),
  },
  data() {
    return {
      searchUser: "",
      currentComponent: null,
      componentData: null,
    };
  },
  computed: {
    userData() {
      console.log("userData", this.$store.getters["user/users"]);
      return this.$store.getters["user/users"];
    },
    //当前的用户
    currentUser() {
      console.log(this.$store.getters["user/currentUser"]);
      return this.$store.getters["user/currentUser"];
    },
  },
  created() {
    this.$store.dispatch("user/getUsers", { pageindex: 1, pageSize: 10 });
  },
  methods: {
    //1.0
    getUser(id) {
      this.$store.dispatch("user/getUser", id);
    },
    addUser() {
      console.log("添加currentuser", this.currentUser);
      if (this.currentUser !== "AddUser") {
        this.$nextTick(() => {
          this.currentComponent = "AddUser";
        });
      }
    },
  },
  watch: {
    //点击当前用户弹窗
    "currentUser.id"(n) {
      console.log("n", n);
      if (n) {
        this.currentComponent = "UserInfo";
        this.componentData = this.currentUser;
        if (n == "00000000-e593-4ef3-ab9b-0a4d74b76ec5") {
          this.currentComponent = AddUser;
          this.componentData = {};
        }
      } else {
        this.currentComponent = null;
        this.componentData = {};
      }
    },
    //检测组件
    currentComponent(n2) {
      console.log("n2", n2);
      if (n2 == "AddUser") {
        var user = {
          data: {
            id: "00000000-e593-4ef3-ab9b-0a4d74b76ec5",
            avatar: "",
            roles: [],
            createDate: "2021-01-16T09:28:15.477",
            lastDate: "2021-01-16T09:28:15.477",
            isInit: false,
          },
        };
        console.log("lou", user);
        this.$store.commit("user/getUser", user);
      } else {
        return;
      }
    },
  },
};
</script>

<style lang="scss" scoped>
#users {
  padding: 10px;
  height: 100%;
  .usersWrapper {
    display: flex;
    height: 100%;
    .usersList {
      position: relative;
      display: flex;
      flex-direction: column;
      border-radius: 8px;
      z-index: 2; //层级
      .searchWrapper {
        margin-bottom: 10px;
      }
      .list {
        background-color: #ffffff;
        flex: 1;
        border-radius: 8px;
        overflow: hidden;
        .listWrapper {
          padding-bottom: 30px;
          height: 100%;
        }
        .ListFoot {
          position: relative;
          margin-top: -30px;
          padding: 0 30px;
          height: 30px;
          p {
            line-height: 30px;
            font-size: 14px;
            color: #707070;
            text-align: center;
          }
          .iconWrapper {
            display: flex;
            justify-content: center;
            align-items: center;
            position: absolute;
            top: 0;
            right: 0;
            width: 30px;
            height: 30px;
            background-color: #67c23a;
            cursor: pointer;
            .icon {
              font-size: 14px;
              color: #ffffff;
            }
          }
        }
      }
    }
    .userInfoWrapper {
      flex: 1;
    }
  }
}
</style>