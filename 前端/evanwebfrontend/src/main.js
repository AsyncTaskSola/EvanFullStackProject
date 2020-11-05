import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import './plugins/element.js'
//自定义js
import JsNewGuid from './customizejs/Guid.js'
import getNowFormatDate from './customizejs/GetNowFormatDate.js'
import './customizejs/EmlDialog.js'
//导入全局样式表
import './assets/css/global.css'
import { Message } from 'element-ui';

//配置全局的axios，挂载在原型上
import axios from 'axios'
axios.defaults.baseURL = 'http://localhost:6001'

//请求拦截器
axios.interceptors.request.use(config => {
  if (!sessionStorage.getItem('Authorization')) {
    console.log("请求拦截器", config);
    return config;
  }
  config.headers.Authorization = sessionStorage.getItem('Authorization');
  return config;
})

//响应拦截器
axios.interceptors.response.use(res => {
  console.log("响应拦截器成功", res);
  // 成功响应
  Message({
    type: res.data.state === 'success' ? 'success' : 'error',
    message: res.data.message
  });
  return res;
},
  err => {
    //错误响应
    console.log("响应拦截器失败", err.message);
    if (err.message.indexOf("401") !== -1) {
      Message({
        type: 'error',
        message: "accessToken不正确或已过期"
      });
    }
    else {
      Message({
        type: res.data.state === 'error',
        message: `${res.data.message}(状态码:${res.res.status})`
      });
    }
  })
Vue.prototype.$http = axios

//自定义guid生成
Vue.prototype.$guid = JsNewGuid;
Vue.prototype.$Gettime = getNowFormatDate;





Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
