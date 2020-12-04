import state from './state';
import actions from './actions';
import mutations from './mutations';
import getters from './getters';

export default {
	namespaced: true, // 命名空间 重点
	state,
	actions,
	mutations,
	getters
};