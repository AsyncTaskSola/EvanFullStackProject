import Vue from 'vue'
import VueRouter from 'vue-router'
// import Home from '../views/Home.vue'
//import Login from '@/components/Login.vue'
const Login = () =>
    import ( /* webpackChunkName: "login_home——welcome" */ '@/components/Login.vue')
Vue.use(VueRouter)

// // const routes = [
// //   {
// //     path: '/',
// //     name: 'Home',
// //     component: Home
// //   },
// //   {
// //     path: '/about',
// //     name: 'About',
// //     // route level code-splitting
// //     // this generates a separate chunk (about.[hash].js) for this route
// //     // which is lazy-loaded when the route is visited.
// //     component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
// //   } 
// // ]

const router = new VueRouter({
  routes: [{
    path: "/",
    redirect: '/Login'
  },
  {
    path: "/Login",
    component: Login
  },]
})

export default router
