import state from "../user/state";

const getters = {
    errorRes: state => state.errorRes,
    errorMsg: state => state.errorMsg,
    menus: state => state.menus,
    menusMap: state => state.menusMap,
    fold: state => state.fold
};

export default getters;