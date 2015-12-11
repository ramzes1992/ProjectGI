var files = ['/../Data/pkb.csv']
 var loadData = function(i,x,y,svg,height,width,xAxis,yAxis){
  var clas = files[i].substring(files[i].lastIndexOf("/")+1,files[i].indexOf('.csv'))
  d3.csv(files[i], type, function(error, data) {
  if (error) throw error;


  x.domain(data.map(function(d) { return d.year; }));
  y.domain([0, 10]);

svg.append("g")
      .attr("class", "x axis")
      .attr("transform", "translate(0," + (height + 25 )+ ")")
      .call(xAxis);

  svg.append("g")
      .attr("class", "y axis")
      .call(yAxis)
    .append("text")
      .attr("transform", "rotate(-90)")
      .attr("y", 6)
      .attr("dy", ".71em")
      .style("text-anchor", "end");


var line = d3.svg.line()
    .x(function(d) { return x(d.year); })
    .y(function(d) { return y(d.value); });

svg.selectAll("."+clas)
      .data(data)
    .enter().append("circle")
      .attr("class", clas)
      .attr("cx", function(d) { return x(d.year); })
      .attr("cy", function(d) { return y(d.value); })
      .attr("r",10);
    
   

  svg.append("path")
      .datum(data)
      .attr("class", "line "+clas)
      .attr("d", line);
  if(i<files.length-1)
    loadData(++i,x,y,svg,height,width,xAxis,yAxis);
});
}
var chart = function(dataFiles){
var margin = {top: 20, right: 20, bottom: 50, left: 40},
    width = 960 - margin.left - margin.right,
    height = 500 - margin.top - margin.bottom;

var x = d3.scale.ordinal()
    .rangePoints([0, width], .1);

var y = d3.scale.linear()
    .range([0,height]);

var xAxis = d3.svg.axis()
    .scale(x)
    .outerTickSize(0)
    .orient("bottom");

var yAxis = d3.svg.axis()
    .scale(y)
    .orient("left")
    .ticks(10);


var svg = d3.select("div#chart").html("").append("svg")
    .attr("width", width + margin.left + margin.right)
    .attr("height", height + margin.top + margin.bottom)
  .append("g")
    .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

loadData(0,x,y,svg,height,width,xAxis,yAxis);




}
function type(d) {
  d.value = +d.value;
  d.year = +d.year;
  return d;
}
var toggleData = function(data){
    if (data !== undefined) {
        if(_.contains(files, data)){
            if (files.length > 1)
            files.splice(files.indexOf(data), 1);
        } else {
            files.push(data);
        }
    }
}

chart(files)
_.each($('li'),function(li){
    $(li).on("click", function () {
        var tmp = files.length;
        toggleData($(li).attr('data'))
        if(tmp != files.length)
            chart(files)
  })
})
