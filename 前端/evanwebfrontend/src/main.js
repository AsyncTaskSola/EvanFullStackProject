import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import './plugins/element.js'

//导入全局样式表
import './assets/css/global.css'

//配置全局的axios，挂载在原型上
import axios from 'axios'
axios.defaults.baseURL='http://localhost:6001'

//请求拦截器
axios.interceptors.request.use(config=>{
  if(!sessionStorage.getItem('Authorization'))
  {
    console.log("请求拦截器",config);  
    return config;
  }
  config.headers.Authorization = sessionStorage.getItem('Authorization');
  return config;
})
Vue.prototype.$http=axios

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
