import Cell from './src';

const CellComponent={
    install(vue){
        vue.component(Cell.name, Cell);
    }
}
export default CellComponent;