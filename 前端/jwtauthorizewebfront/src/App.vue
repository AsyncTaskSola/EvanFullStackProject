<template>
<div id="app">
		<!-- <transition :name="pageAnimation"> -->
			<!-- <keep-alive :include="keepAlive"> -->
				<router-view />
			<!-- </keep-alive> -->
		<!-- </transition> -->
	</div>
</template>

<script>


export default {
  name: 'app',
  data() {
		return {
			pageAnimation: ''
		};
	},
}
</script>

<style lang="scss">
@import '@/assets/css/app.scss';
</style>
