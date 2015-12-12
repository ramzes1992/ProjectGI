
  // canvas resolution
  var width = 1000,
      height = 600;
 
  // projection-settings for mercator    
  var projection = d3.geo.mercator()
      // where to center the map in degrees
      .center([22, 61 ])
      // zoomlevel
      .scale(500)
      // map-rotation
      .rotate([0,0]);
 
  // defines "svg" as data type and "make canvas" command
  var svg = d3.select("body").append("svg")
      .attr("width", width)
      .attr("height", height);
 
  // defines "path" as return of geographic features
  var path = d3.geo.path()
      .projection(projection);
 
  // group the svg layers 
  var g = svg.append("g");
 d3.json('getgdpdata', function(error, data) {
  if (error) throw error;
 //  load data and display the map on the canvas with country geometries
  d3.json("https://gist.githubusercontent.com/d3noob/5193723/raw/world-110m2.json", function(error, topology) {
      
      var max = _.max(data, function (x) { return x.Value; });
      var min = _.min(data, function (x) { return x.Value; });
      var avg = d3.sum(data, function (x) { return x.Value; }) / data.length;
      g.selectAll("path")
        .data(topojson.object(topology, topology.objects.countries)
            .geometries)
      .enter()
        .append("path")
        .attr("d", path)
        .attr("class", "country")
        .attr("id", function (d) { return d.id })
              .style("fill", function (d) {
                  var cdata = _.find(data, function (x) { return x.CountryCode == d.id; });
                  var color = undefined;
                  if (cdata == undefined)
                      return "grey";
                  if ( cdata.Value > avg)
                      color = d3.rgb(0, 51, 0).darker((cdata.Value / max.Value));
                  else
                      color = d3.rgb(255, 0, 0).darker((min.Value / cdata.Value));
                  return color;
              });
  });
  
  });
  // zoom and pan functionality
  /*var zoom = d3.behavior.zoom()
      .on("zoom",function() {
          g.attr("transform","translate("+ 
              d3.event.translate.join(",")+")scale("+d3.event.scale+")");
          g.selectAll("path")  
              .attr("d", path.projection(projection)); 
    });
 
  svg.call(zoom)*/
 
 function type(d){
     d.CountryCode = +d.CountryCode;
  d.Value =+d.Value;
  return d;
 }