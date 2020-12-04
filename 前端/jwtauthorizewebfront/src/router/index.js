import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'login',
    mata:{
      auth:false,
      index:1
    },
    component:()=>import(/* webpackChunkName: "login" */ '../views/login/login.vue') 
  },
  {
    path:'/home',
    name:'home',
    mata:{
      auth:true,
      index:2
    },
    component:()=>import(/* webpackChunkName: "home" */ '../views/home/home.vue') 
  }
]

const router = new VueRouter({
  mode: 'history',
  routes
})

export default router
