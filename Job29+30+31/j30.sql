SELECT
  name_1,
  ST_X(ST_Centroid(geom)) AS lon,
  ST_Y(ST_Centroid(geom)) AS lat
FROM
  gadm41_VNM_1
  
  
SELECT ST_ASGeoJSON(geom) FROM vnm_rdsl_2015_osm

SELECT g.name_1, ST_ASGeoJSON(o.geom)
FROM vnm_rdsl_2015_osm AS o
JOIN gadm41_VNM_1 AS g ON ST_Within(o.geom, g.geom)
WHERE g.name_1 = 'Hà Nội';

-- Truy vấn trong bảng gadm41_vnm_1 để tìm các đối tượng không gian chứa điểm được tạo
SELECT name_1, ST_AsGeoJSON(geom) AS geometry
FROM gadm41_vnm_1
WHERE ST_Contains(geom, ST_MakePoint(104.9545187709354, 19.197053439464863));

SELECT name_1, ST_Area(geom) AS area
FROM gadm41_vnm_1
WHERE ST_Area(geom) > 0
ORDER BY area DESC
