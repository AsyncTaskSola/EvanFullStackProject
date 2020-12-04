<template>
  <div class="topBar">
      <div class="pageInfo">
          <div class="info">
              
          </div>
      </div>
  </div>
</template>

<script>
export default {

}
</script>

<style lang="scss" scoped>
.topBar{
    position: relative;
    padding: 15px 20px;
    display: flex;
    align-items: center;
    &:after {
		content: '';
		position: absolute;
		right: 0;
		bottom: 0;
		left: 0;
		border-bottom: 1px solid black;
		transform: scaleY(0.5);
	}
}
</style>