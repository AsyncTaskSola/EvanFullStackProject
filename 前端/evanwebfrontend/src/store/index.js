import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

// 这部分好像用不上，哈哈哈哈，本来打算写在全局获取返回的数据的

export default new Vuex.Store({
  state: {
    AccessToken:''
  },
  // 同步这个主要可以修改上面的数据state
  mutations: {
    // 给AccessToken赋值
    SetAccessTokenValue(state,receviceParmer)
    {
      state.AccessToken=receviceParmer;
      // localStorage
      
    }
  },

  //异步  只能通过context.commit 操作mutations的某个方法改变数字，自身不能直接改数据
  actions: {
   GetAccessTokenValue(context){
    this.$http
    .post(
      "/api/LoginUserInfo/GetInfo",
      {},
      {
        headers: {
          Authorization: `Bearer ${sessionStorage.getItem("Authorization")}`,
        },
      }
    ).then(res=>{
      context.commit('SetAccessTokenValue',res.data.data.accessToken)
    })
   }
  },
  modules: {
  }
})
