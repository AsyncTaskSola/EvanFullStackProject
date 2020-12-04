import services from '../../api';
import * as types from './types'  //注释 export

const mutations={
	//记录登陆时的用户
	[types.SET_USER_INFO](state,userInfo){
		sessionStorage.setItem("userInfo",JSON.stringify(userInfo));
		state.userInfo=userInfo;
	},

	//登出
	[types.LOGOUT](state)
	{
		sessionStorage.removeItem("Authorization");
		sessionStorage.removeItem("userInfo");
		state.userInfo={};
	},
	//重设密码
	[types.SET_USER_RESETPASSWORD](state)
	{
		state.currentUser = {};
	}
}


export default mutations;
