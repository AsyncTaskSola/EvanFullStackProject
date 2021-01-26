import Vue from 'vue';
import Vuex from 'vuex';
import global from './global'; // 全局状态
import user from './user';
import role from './role'

Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        global,
        user,
        role
    },
    strict: process.env.NODE_ENV !== 'production'
});