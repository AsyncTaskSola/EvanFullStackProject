import axios from 'axios'
import store from '../store'
import { Message } from 'element-ui';
import router from '../router';

// api
//axios.defaults.baseURL = process.env.VUE_APP_API;
axios.defaults.baseURL = "http://localhost:7010/api"
//请求超时
axios.defaults.timeout = 100000;

//全局请求拦截器
axios.interceptors.request.use(config => {
    //默认是false
    console.log("全局拦截器vuex调用", store.getters);
    if (store.getters['global/errorRes']) {
        let msg = store.getters['global/errorMsg'];
        Message({
            type: 'error',
            message: msg
        });
        return Promise.reject(new Error(msg))
    } else {
        // 添加认证头
        console.log("添加认证头");
        if (sessionStorage.getItem('Authorization')) {
            config.headers.Authorization =
                'Bearer ' + sessionStorage.getItem('Authorization');
        }
        return config;
    }
}, err => {
    return Promise.reject(err);
})


//全局响应拦截器
axios.interceptors.response.use(res => {
    console.log('成功响应', res.data);
    //成功响应
    Message({
        type: res.data.state === 'success' ? 'success' : 'error',
        message: res.data.message
    });
    return res;
},
    err => { 
        const res = err.response;
        var message=res.data.Message?res.data.Message:res.statusText
        if (!store.getters['global/errorRes']) {
            //错误响应
            Message({
                type: 'error',
                message: `${message} 状态码(${res.status})`
            });                   
            if(res.status===401)
            {
                router.push('/');// 精髓
            }
            store.commit('global/errorRes', {
                status: true,
                msg: message
            });
        }
        return Promise.reject(err.message);
    }
)
export default axios;
