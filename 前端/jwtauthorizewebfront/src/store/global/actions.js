import * as types from './types';
import api from '@/api'
const actions={
  async [types.SET_MENUS]({commit}){
      let menus=await api.page.getMenu();
      console.log("获取菜单",menus);
      commit(types.SET_MENUS,menus);
  }
};
export default actions;