import state from "./state";
import mutations from "./mutations";
import actions from "./actions";
import getters from "./getters";

export default {
  namespaced: true, // 命名空间
  state,
  mutations,
  actions,
  getters
};