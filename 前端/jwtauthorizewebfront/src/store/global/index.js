import state from './State';
import mutations from './mutations';
import getters from './getters';
import actions from './actions';

export default {
    namespaced: true, // 命名空间
    state,
    mutations,
    getters,
    actions
};