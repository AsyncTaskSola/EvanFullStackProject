import Vue from 'vue'
import VueRouter from 'vue-router'
// import Home from '../views/Home.vue'
//import Login from '@/components/Login.vue'
const Login = () =>
  import('@/components/Login.vue')
const Home = () =>
  import('@/components/Home.vue')
const Welcome = () =>
  import('@/components/Welcome.vue')
const Users = () =>
  import('@/components/Users/Users.vue')
const UsersInfo = () =>
  import('@/components/Users/UsersInfo.vue')
const CompaniesInfo = () =>
  import('@/components/Companies/CompaniesInfo.vue')
const EmployeesInfo = () =>
  import('@/components/Employees/EmployeesInfo.vue')
const AdminArea = () =>
  import('@/components/AdminAreas/AdminArea.vue')
const Performance = () =>
  import('@/components/Performances/Performance.vue')
Vue.use(VueRouter)


const router = new VueRouter({
  routes: [{
    path: "/",
    redirect: '/Login'
  },
  {
    path: "/Login",
    component: Login
  },
  {
    path: "/Home",
    component: Home,
    redirect: "/Welcome",
    children: [
      {
        path: "/Welcome", component: Welcome,
      },
      {
        path: "/Users", component: Users,
      },
      {
        path: "/UsersInfo", component: UsersInfo
      },
      {
        path: "/CompaniesInfo", component: CompaniesInfo
      }, {
        path: "/EmployeesInfo", component: EmployeesInfo
      }, {
        path: "/AdminArea", component: AdminArea
      }, {
        path: "/Performance", component: Performance
      }]
  },]
})

//挂载路由导航守卫，看登陆后的页面是否带有Authorization值，来判断是否有查看权限
router.beforeEach((to, from, next) => {
  // to将要访问的路径,from 代表从哪个路径跳转而来 next 放行 next(”路由地址“) 强制跳转
  if (to.path === '/Login') return next()
  const accesstoken = sessionStorage.getItem("Authorization");
  if (!accesstoken) {
    return next("/Login");
  }
  next();
})


export default router
