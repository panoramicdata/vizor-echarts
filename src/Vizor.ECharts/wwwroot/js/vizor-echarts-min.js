window.vizorECharts={charts:new Map,logging:!1,changeLogging:function(t){vizorECharts.logging=t},getChart:function(t){return vizorECharts.charts.get(t)},evaluatePath:function(t,a){const i=a.split(".");let e=t;for(const t of i){if(!e.hasOwnProperty(t))return;e=e[t]}return e},fetchExternalData:async function(t,a){if(null!=a)for(item of JSON.parse(a)){vizorECharts.logging&&(console.log("FETCH"),console.log(item));const a=await fetch(item.url,item.options);if(!a.ok)throw new Error("Failed to fetch external chart data: url="+url);var i=null;"json"==item.fetchAs?(i=await a.json(),vizorECharts.logging&&console.log(i),null!=item.path&&(i=vizorECharts.evaluatePath(i,item.path))):"string"==item.fetchAs&&(i=await a.text()),window.vizorECharts.charts.get(t)[item.id]=i}},registerMaps:function(chartId,mapOptions){if(null!=mapOptions){var parsedOptions=eval("("+mapOptions+")");for(item of parsedOptions)vizorECharts.logging&&(console.log("MAP"),console.log(item)),"geoJSON"===item.type?echarts.registerMap(item.mapName,{geoJSON:item.geoJSON,specialAreas:item.specialAreas}):"svg"===item.type&&echarts.registerMap(item.mapName,{svg:item.svg})}},initChart:async function(id,theme,initOptions,chartOptions,mapOptions,fetchOptions){var chart=echarts.init(document.getElementById(id),theme,JSON.parse(initOptions));if(vizorECharts.charts.set(id,chart),chart.showLoading(),null!=chartOptions){await vizorECharts.fetchExternalData(id,fetchOptions),await vizorECharts.registerMaps(id,mapOptions);var parsedOptions=eval("("+chartOptions+")");vizorECharts.logging&&(console.log("CHART"),console.log(parsedOptions)),chart.setOption(parsedOptions),chart.hideLoading()}},updateChart:async function(id,chartOptions,mapOptions,fetchOptions){var chart=vizorECharts.charts.get(id);if(null!=chart){await vizorECharts.fetchExternalData(id,fetchOptions),await vizorECharts.registerMaps(id,mapOptions);var parsedOptions=eval("("+chartOptions+")");chart.setOption(parsedOptions),chart.hideLoading()}else console.error("Failed to retrieve chart "+id)},disposeChart:function(t){var a=vizorECharts.charts.get(t);null!=a?(echarts.dispose(a),vizorECharts.charts.delete(t)):console.error("Failed to dispose chart "+t)}};