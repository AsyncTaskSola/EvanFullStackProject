const state = {
	userInfo: sessionStorage.getItem('userInfo')
		? JSON.parse(sessionStorage.getItem('userInfo'))
		: {},
	users: [],
	currentUser: {}
};
export default state;